namespace NewsAggregatorConsoleUI;

public interface INetworkManager
{
    Task<string> GetResponseString(string url);
}