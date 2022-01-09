using Common.Models;

namespace Common.Services;

public interface IHtmlParserService
{
    Task<string> ProcessArticleBody(ArticleRootModel article);
}