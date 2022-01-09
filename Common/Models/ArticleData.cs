using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace Common.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ArticleData
{
    public Article Root { get; set; }
}