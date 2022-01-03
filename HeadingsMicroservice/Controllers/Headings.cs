using HeadingsMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HeadingsMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Headings
{
#pragma warning disable CS8618
    private static NetworkManager _networkManager;
#pragma warning restore CS8618

    public Headings()
    {
        _networkManager = new NetworkManager();
    }

    [HttpGet("{category}/{count:int}")]
    public async Task<IEnumerable<Heading>> Get(string category = "news", int count = 10)
    {
        var headings = await GetHeadings(category);
        return headings.Take(count);
    }

    private static async Task<IEnumerable<Heading>> GetHeadings(string category)
    {
        var response = await _networkManager.GetResponse($"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page=0&per_page=100");
        var headings = JsonConvert.DeserializeObject<Data>(response);
        return GetHeadingsList(category, headings);
    }

    private static IEnumerable<Heading> GetHeadingsList(string category, Data? headings)
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