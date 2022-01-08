namespace NewsAggregatorWebWASMUI.Services;

public interface ICache
{
    Dictionary<string, string> Cache { get; set; }
}