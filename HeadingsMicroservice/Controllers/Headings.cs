using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HeadingsMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Headings
{
    private readonly INetworkService _networkService;

    public Headings(INetworkService networkService)
    {
        _networkService = networkService;
    }

    [HttpGet("{category}")]
    public async Task<IEnumerable<Heading>> Get(string category = "news")
    {
        var headings = await GetHeadings(category);
        return headings;
    }

    private async Task<IEnumerable<Heading>> GetHeadings(string category)
    {
        var response = await _networkService.GetResponse($"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page=0&per_page=24");
        var headings = JsonConvert.DeserializeObject<HeadingsData>(response);
        return GetHeadingsList(category, headings);
    }

    private static IEnumerable<Heading> GetHeadingsList(string category, HeadingsData? headings)
    {
        IEnumerable<Heading>? documents;
        if (category.Equals("news", StringComparison.InvariantCultureIgnoreCase))
        {
            documents = headings?.Documents
                .Where(pair => pair.Value.Tag["name"]
                    .Equals("новости", StringComparison.InvariantCultureIgnoreCase))
                .Select(pair => pair.Value);
        }
        else
        {
            documents = headings?.Documents.Select(pair => pair.Value);
        }

        return documents ?? Array.Empty<Heading>();
    }
}