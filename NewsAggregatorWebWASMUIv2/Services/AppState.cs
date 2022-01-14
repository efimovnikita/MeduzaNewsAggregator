using Common.Models;

namespace NewsAggregatorWebWASMUIv2.Services;

public class AppState : IState
{
    public List<FullArticle> Articles { get; private set; } = new();
    public List<FullArticle> SearchedArticles { get; private set; } = new();
    
    public void AddNewArticle(FullArticle article)
    {
        var existing = Articles.FirstOrDefault(fullArticle => fullArticle.Title.Equals(article.Title));
        if (existing == null)
        {
            Articles.Add(article);
        }
        NotifyStateChanged();
    }

    public void AddNewArticles(List<FullArticle> articles)
    {
        foreach (var article in articles)
        {
            AddNewArticle(article);
        }
        NotifyStateChanged();
    }

    public void AddSearchedArticles(List<FullArticle> articles)
    {
        SearchedArticles = articles;
        NotifyStateChanged();
    }
    public void AddTextToSimpleArticle(string title, string text)
    {
        var article = Articles.FirstOrDefault(article => article.Title.Equals(title));
        if (article != null)
        {
            article.Text = text;
        }
        NotifyStateChanged();
    }
    
    public void AddTextToSearchArticle(string title, string text)
    {
        var article = SearchedArticles.FirstOrDefault(article => article.Title.Equals(title));
        if (article != null)
        {
            article.Text = text;
        }
        NotifyStateChanged();
    }
    
    public event Action? StateChanged;

    private void NotifyStateChanged() => StateChanged?.Invoke();
}