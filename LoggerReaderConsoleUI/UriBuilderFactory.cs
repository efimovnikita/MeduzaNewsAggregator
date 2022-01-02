namespace LoggerReaderConsoleUI;

public class UriBuilderFactory : IUriBuilderFactory
{
    public UriBuilder GetBuilder(string url)
    {
        return new UriBuilder(url);
    }
}