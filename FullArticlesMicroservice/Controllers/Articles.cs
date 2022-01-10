using System.Text;
using Common.Models;
using Common.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FullArticlesMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Articles
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHtmlParserService _htmlParserService;

    public Articles(IHttpClientFactory httpClientFactory, IHtmlParserService htmlParserService)
    {
        _httpClientFactory = httpClientFactory;
        _htmlParserService = htmlParserService;
    }
    
    [HttpGet("{url}")]
    public async Task<string> Get(string url)
    {
        return await GetArticle(url);
    }

    private async Task<string> GetArticle(string url)
    {
        var httpClient = _httpClientFactory.CreateClient("articles");
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://meduza.io/api/v3/{url}"));
        if (responseMessage.IsSuccessStatusCode == false) return string.Empty;

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var articleRootModel = GetArticleRootModel(contentString);
        if (articleRootModel == null) return string.Empty;

        await SendPostRequest(articleRootModel, httpClient);

        return await _htmlParserService.ProcessArticleBody(articleRootModel);
    }

    private static ArticleRootModel? GetArticleRootModel(string contentString)
    {
        ArticleRootModel? article;

        try
        {
            article = JsonConvert.DeserializeObject<ArticleRootModel>(contentString);
        }
        catch
        {
            return null;
        }
        
        return article;
    }

    private static async Task SendPostRequest(ArticleRootModel? article, HttpClient httpClient)
    {
        if (article != null)
        {
            var logMessageModel = new LogMessageModel(article.Root.Title);
            var postRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://logs-river.herokuapp.com/Logs");
            postRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(logMessageModel), Encoding.Default,
                "application/json");
            await httpClient.SendAsync(postRequestMessage);
        }
    }
}