using System.Diagnostics.CodeAnalysis;

namespace NewsAggregatorWebWASMUI.Models;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class Data
{
    public Dictionary<string, Heading> Documents { get; } = new();
}