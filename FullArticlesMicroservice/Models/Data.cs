using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace FullArticlesMicroservice.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Data
{
    public Article Root { get; set; }
}