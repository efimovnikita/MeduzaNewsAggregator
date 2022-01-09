using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Heading
{
    public string Title { get; set; } = string.Empty;
    public DateTime Pub_Date { get; set; } = DateTime.Now;
    public string Url { get; set; } = string.Empty;
    public Dictionary<string, string> Tag { get; set; } = new();
}