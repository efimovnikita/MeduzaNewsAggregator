using System.Net;
using Common.HelperMethods;
using Common.Models;
using Common.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddHttpClient("headings")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
builder.Services.AddSingleton<IStorageService, HeadingsStorageService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.UseSwagger();

app.MapGet("/{category}",
    async (HttpContext context, IStorageService storage, IHttpClientFactory httpClientFactory, string category) =>
    {
        var searchString = context.Request.Query["search"].ToString();

        var tagNameBasedOnCategory = Methods.GetTagNameBasedOnCategory(category);
        var models = storage.HeadingModels.Where(model =>
        {
            model.Tag.TryGetValue("name", out var nameTagValue);
            return string.IsNullOrWhiteSpace(nameTagValue) != true && nameTagValue.Equals(tagNameBasedOnCategory,
                StringComparison.InvariantCultureIgnoreCase);
        }).ToList();
        
        if (models.Count > 0)
        {
            return SearchFromModels(storage.HeadingModels, searchString);
        }

        var httpClient = httpClientFactory.CreateClient("headings");
        var headingModels = new List<HeadingModel>();
        for (var i = 0; i < 60; i++)
        {
            var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page={i}&per_page=50"));

            if (responseMessage.IsSuccessStatusCode != true) continue;
            var contentString = await responseMessage.Content.ReadAsStringAsync();
            var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel>(contentString);
            var headingsList = Methods.GetHeadingsList(category, headingsData).ToList();
            headingModels.AddRange(headingsList);
        }
    
        storage.HeadingModels = headingModels;
        return SearchFromModels(storage.HeadingModels, searchString);
    });

List<HeadingModel> SearchFromModels(IEnumerable<HeadingModel> list, string searchString)
{
    var searchedModels = list
        .Where(model => model.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
        .ToList();
        
    return searchedModels;
}

app.UseSwaggerUI();

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyHeader());

app.Run();