namespace Common.Models;

public class FullArticle
{
    public FullArticle(string title, string url, DateTime date, int publishedAt, Dictionary<string, string> tag)
    {
        Title = title;
        Url = url;
        PublishedAt = publishedAt;
        Tag = tag;
        Date = date;
    }
    public string Title { get; private set; }
    public string Url { get; private set; }
    public int PublishedAt { get; private set; }
    public DateTime Date { get; set; }
    
    public Dictionary<string, string> Tag { get; set; }
    public string Text { get; set; } = string.Empty;
}

