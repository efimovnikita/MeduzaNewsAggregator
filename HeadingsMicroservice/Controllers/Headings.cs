using System.IO.Compression;
using HeadingsMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HeadingsMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Headings
{
    [HttpGet("{category}/{count:int}")]
    public async Task<IEnumerable<Heading>> Get(string category = "news", int count = 10)
    {
        var headings = await GetHeadings(category);
        return headings.Take(count);
    }

    private static async Task<IEnumerable<Heading>> GetHeadings(string category)
    {
        var uri = $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page=0&per_page=100";
        var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(uri);
        var json = await new StreamReader(new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            .ReadToEndAsync();
        var headings = JsonConvert.DeserializeObject<Data>(json);
        
        IEnumerable<Heading>? documents;
        if (category.Equals("news", StringComparison.InvariantCultureIgnoreCase))
        {
            documents = headings?.Documents
                .Where(pair => pair.Value.Tag["name"].Equals("новости", StringComparison.InvariantCultureIgnoreCase))
                .Select(pair => pair.Value);
        }
        else
        {
            documents = headings?.Documents.Select(pair => pair.Value);
        }
        
        return documents ?? new List<Heading>();
    }
}