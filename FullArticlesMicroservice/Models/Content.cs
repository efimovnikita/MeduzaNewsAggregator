using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace FullArticlesMicroservice.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Content
{
    public string Body { get; set; }
}