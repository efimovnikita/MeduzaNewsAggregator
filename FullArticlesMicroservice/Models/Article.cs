using System.Diagnostics.CodeAnalysis;

namespace FullArticlesMicroservice.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Article
{
    public string Title { get; set; } = string.Empty;
    public Dictionary<string, string> Source { get; set; } = new();
    public Content Content { get; set; }
}