using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class HeadingsData
{
    public Dictionary<string, Heading> Documents { get; } = new();
}