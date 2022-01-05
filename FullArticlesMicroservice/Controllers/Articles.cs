using FullArticlesMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HeadingsMicroservice.Controllers;

namespace FullArticlesMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Articles
{
    private readonly INetworkService _networkService;
    private readonly IHtmlParser _htmlParser;

    public Articles(INetworkService networkService, IHtmlParser htmlParser)
    {
        _networkService = networkService;
        _htmlParser = htmlParser;
    }
    
    [HttpGet("{url}")]
    public async Task<string> Get(string url)
    {
        return await GetArticle(url);
    }

    private async Task<string> GetArticle(string url)
    {
        var json = await _networkService.GetResponse($"https://meduza.io/api/v3/{url}");
        var article = JsonConvert.DeserializeObject<Data>(json);
        
        if (article is null)
        {
            return string.Empty;
        }
        
        await _networkService.SendPostRequest($"https://logs-river.herokuapp.com/Logs?item={article.Root.Title}");
        return await _htmlParser.ProcessArticleBody(article);
    }
}