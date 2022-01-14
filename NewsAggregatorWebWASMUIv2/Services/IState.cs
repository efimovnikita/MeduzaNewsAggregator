using Common.Models;

namespace NewsAggregatorWebWASMUIv2.Services;

public interface IState
{
    List<FullArticle> Articles { get; }
    List<FullArticle> SearchedArticles { get; }
    void AddNewArticles(List<FullArticle> articles);
    void AddSearchedArticles(List<FullArticle> articles);
    void AddTextToSimpleArticle(string title, string text);
    void AddTextToSearchArticle(string title, string text);
    event Action? StateChanged;
}