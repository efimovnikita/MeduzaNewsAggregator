using FullArticlesMicroservice.Models;

namespace FullArticlesMicroservice.Services;

public interface IHtmlParserService
{
    Task<string> ProcessArticleBody(Data article);
}