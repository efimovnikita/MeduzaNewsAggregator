using FullArticlesMicroservice.Models;

namespace FullArticlesMicroservice.Controllers;

public interface IHtmlParser
{
    Task<string> ProcessArticleBody(Data article);
}