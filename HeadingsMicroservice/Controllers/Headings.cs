using Common.HelperMethods;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HeadingsMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Headings
{
    private readonly IHttpClientFactory _httpClientFactory;

    public Headings(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{category}")]
    public async Task<IEnumerable<HeadingModel>> Get(string category = "news")
    {
        var headings = await GetHeadings(category);
        return headings;
    }

    private async Task<IEnumerable<HeadingModel>> GetHeadings(string category)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page=0&per_page=24"));

        if (responseMessage.IsSuccessStatusCode == false)
        {
            return new List<HeadingModel>();
        }

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel>(contentString);
        return Methods.GetHeadingsList(category, headingsData);
    }
}