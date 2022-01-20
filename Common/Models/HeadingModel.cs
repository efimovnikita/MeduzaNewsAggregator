// ReSharper disable All
namespace Common.Models;

public class HeadingModel
{
    public string Title { get; set; } = string.Empty;
    public DateTime Pub_Date { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    public Dictionary<string, string> Tag { get; set; } = new();
    public string Document_Type { get; set; } = string.Empty;
    public int Published_At { get; set; } = 0;
}

public class ModelTag
{
    public string Name { get; set; } = String.Empty;
    public string Path { get; set; } = String.Empty;
}