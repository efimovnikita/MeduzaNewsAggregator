using System.Diagnostics.CodeAnalysis;

namespace Common.Models;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class HeadingsDataModel
{
    public Dictionary<string, HeadingModel> Documents { get; } = new();
}