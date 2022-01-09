using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618

namespace Common.Models;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Article
{
    public string Title { get; set; } = string.Empty;
    public Dictionary<string, string> Source { get; set; } = new();
    public Content Content { get; set; }
}