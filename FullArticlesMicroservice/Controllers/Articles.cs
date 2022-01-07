using FullArticlesMicroservice.Models;
using FullArticlesMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HeadingsMicroservice.Services;

namespace FullArticlesMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Articles
{
    private readonly INetworkService _networkService;
    private readonly IHtmlParserService _htmlParserService;

    public Articles(INetworkService networkService, IHtmlParserService htmlParserService)
    {
        _networkService = networkService;
        _htmlParserService = htmlParserService;
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

        var logMessageModel = new LogMessageModel(article.Root.Title);
        await _networkService.SendPostRequestWithJsonBody("https://logs-river.herokuapp.com/Logs", logMessageModel);
        return await _htmlParserService.ProcessArticleBody(article);
    }
}