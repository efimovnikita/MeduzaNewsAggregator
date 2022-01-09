using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace Common.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class ArticleContentModel
{
    public string Body { get; set; }
}