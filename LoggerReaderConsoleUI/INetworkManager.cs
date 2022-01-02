namespace LoggerReaderConsoleUI;

public interface INetworkManager
{
    Task<List<string>> GetResponseStringList(string url);
}