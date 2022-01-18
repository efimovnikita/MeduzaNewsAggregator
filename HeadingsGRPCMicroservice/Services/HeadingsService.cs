using Common.Models;
using Common.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;

namespace HeadingsGRPCMicroservice.Services;

public class HeadingsService : HeadingsGRPCMicroservice.HeadingsService.HeadingsServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStorageService _storageService;

    public HeadingsService(IHttpClientFactory httpClientFactory, IStorageService storageService)
    {
        _httpClientFactory = httpClientFactory;
        _storageService = storageService;
    }

    public override async Task<HeadingsResponse> GetHeadings(GetHeadingsRequest request, ServerCallContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            $"https://meduza.io/api/v3/search?chrono={request.Category}&locale=ru&page=0&per_page=24"));

        if (responseMessage.IsSuccessStatusCode == false)
        {
            return new HeadingsResponse
            {
                Headings = { new List<Heading>() }
            };
        }

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel2>(contentString);

        var headings = headingsData!.Documents
            .Select(pair => pair.Value)
            .Select(FromInternalModel)
            .ToList();
        return new HeadingsResponse
        {
            Headings = { headings }
        };
    }
    
    public override async Task<HeadingsResponse> SearchHeadings(SearchRequest searchRequest, ServerCallContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var category = searchRequest.Category;
        
        var savedCategoryTuples = _storageService.HeadingModels
            .Where(tuple => tuple.Item1.Equals(category))
            .ToList();

        if (savedCategoryTuples.Count == 0)
        {
            var responseMessagesTasks = GetResponseMessagesTasks(httpClient, category);
            var responseMessages = await Task.WhenAll(responseMessagesTasks);

            var contentStringTasks = responseMessages
                .Select(responseMessage => responseMessage.Content.ReadAsStringAsync())
                .ToList();

            var contentStrings = await Task.WhenAll(contentStringTasks);

            var deserializedModelsTasks = contentStrings
                .Select(content => Task.Run(() => DeserializeHeadingsData(content)))
                .ToList();

            var models = await Task.WhenAll(deserializedModelsTasks);
            var tuples = ZipCategoriesWithModels(models, category);

            _storageService.HeadingModels.AddRange(tuples);
        }

        var headingsDataModels = _storageService.HeadingModels
            .Where(tuple => tuple.Item2 is not null)
            .Select(tuple => tuple.Item2)
            .ToList();

        var headings = headingsDataModels
            .Select(model2 => model2!.Documents)
            .SelectMany(model2S => model2S.Values)
            .Select(FromInternalModel)
            .ToList();

        var filteredHeadings = headings
            .Where(heading => heading!.Title.Contains(searchRequest.Searchquery, StringComparison.InvariantCultureIgnoreCase))
            .Distinct()
            .ToList();

        return new HeadingsResponse
        {
            Headings = { filteredHeadings }
        };
    }

    private static IEnumerable<(string category, HeadingsDataModel2? model)> ZipCategoriesWithModels(HeadingsDataModel2?[] models, string category)
    {
        List<string> categories = new();
        for (var i = 0; i < models.Length; i++)
        {
            categories.Add(category);
        }

        IEnumerable<(string category, HeadingsDataModel2? model)> tuples = categories.Zip(models);
        return tuples;
    }

    private static HeadingsDataModel2? DeserializeHeadingsData(string content)
    {
        HeadingsDataModel2? headingsData = default;
        try
        {
            headingsData = JsonConvert.DeserializeObject<HeadingsDataModel2>(content);
        }
        catch
        {
            // ignored
        }

        return headingsData;
    }

    private static IEnumerable<Task<HttpResponseMessage>> GetResponseMessagesTasks(HttpClient httpClient, string category)
    {
        List<Task<HttpResponseMessage>> responseMessagesTasks = new();
        for (var i = 0; i < 150; i++)
        {
            responseMessagesTasks.Add(httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page={i}&per_page=100")));
        }

        return responseMessagesTasks;
    }
    
    public static Heading? FromInternalModel(HeadingModel2? source)
    {
        if (source is null)
        {
            return null;
        }

        source.Tag.TryGetValue("name", out var nameTag);
        var tag = new Tag {Name = nameTag ?? String.Empty, Path = String.Empty};

        return new Heading
        {
            Title = source.Title,
            Pubdate = Timestamp.FromDateTimeOffset(source.Pub_Date),
            Publishedat = source.Published_At,
            Tag = tag,
            Type = source.Document_Type,
            Url = source.Url
        };
    }
}