using Common.Models;
using Common.Services;
using Grpc.Core;
using Newtonsoft.Json;

namespace HeadingsGRPCMicroservice.Services;

public class ArticlesService : HeadingsGRPCMicroservice.ArticlesService.ArticlesServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHtmlParserService _htmlParserService;

    public ArticlesService(IHttpClientFactory httpClientFactory, IHtmlParserService htmlParserService)
    {
        _httpClientFactory = httpClientFactory;
        _htmlParserService = htmlParserService;
    }

    public override async Task<ArticleTextResponse> GetArticleText(GetTextRequest request, ServerCallContext context)
    {
        var article = await GetArticle(request.Url);

        return new ArticleTextResponse
        {
            Text = article
        };
    }
    
    private async Task<string> GetArticle(string url)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://meduza.io/api/v3/{url}"));
        if (responseMessage.IsSuccessStatusCode == false) return string.Empty;

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var articleRootModel = GetArticleRootModel(contentString);
        if (articleRootModel == null) return string.Empty;
        
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
}