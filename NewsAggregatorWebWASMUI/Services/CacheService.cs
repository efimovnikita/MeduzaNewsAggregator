namespace NewsAggregatorWebWASMUI.Services;

public class CacheService : ICache
{
    public Dictionary<string, string> Cache { get; set; } = new();
}