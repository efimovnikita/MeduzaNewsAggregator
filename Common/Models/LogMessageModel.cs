namespace Common.Models;

public class LogMessageModel
{
    public LogMessageModel(string title)
    {
        Title = title;
    }

    public string Title { get; set; }
}