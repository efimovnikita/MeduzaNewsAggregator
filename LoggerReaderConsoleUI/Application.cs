namespace LoggerReaderConsoleUI;

public class Application : IApplication
{
    private readonly INetworkManager _networkManager;

    public Application(INetworkManager networkManager)
    {
        _networkManager = networkManager;
    }
    
    public async void Run()
    {
        var list = await _networkManager.GetResponseStringList("https://logs-river.herokuapp.com/Logs");
        var groups = list.GroupBy(s => s).ToList();
        var groupingHeadings = groups
            .OrderByDescending(grouping => grouping.Count())
            .Select(grouping => grouping.Key).ToList();

        Console.WriteLine("Headings log:");
        Console.WriteLine();

        PrintListToConsole(groupingHeadings);
    }

    private static void PrintListToConsole(IReadOnlyList<string> groupingHeadings)
    {
        for (var i = 0; i < groupingHeadings.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {groupingHeadings[i]}");
        }
    }
}