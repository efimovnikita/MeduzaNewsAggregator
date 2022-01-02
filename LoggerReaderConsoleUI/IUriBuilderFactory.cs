namespace LoggerReaderConsoleUI;

public interface IUriBuilderFactory
{
    UriBuilder GetBuilder(string url);
}