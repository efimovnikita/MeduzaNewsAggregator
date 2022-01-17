namespace Common.Models;

public class FullArticle
{
    public FullArticle(string title, string url, DateTime date, int publishedAt, ModelTag modelTag)
    {
        Title = title;
        Url = url;
        PublishedAt = publishedAt;
        ModelTag = modelTag;
        Date = date;
    }
    public string Title { get; private set; }
    public string Url { get; private set; }
    public int PublishedAt { get; private set; }
    public DateTime Date { get; set; }

    public ModelTag ModelTag { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool Expanded { get; set; }
}

