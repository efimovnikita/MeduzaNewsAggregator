using System.CommandLine;
using Common.HelperMethods;
using Common.Models;

namespace NewsAggregatorConsoleTool;

public class App
{
    private readonly IHttpClientFactory _httpClientFactory;

    public App(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Run(string[] args)
    {
        var argument = new Argument<string>("action", () => "show",
            "Choice of action. You can either \"show\" a list of news headlines or \"search\" for news.");
        var categoryOption = new Option<string>(new[] { "--category" }, () => "news", "News category.");
        var queryOption = new Option<string>(new[] { "--query", "-q" }, "Search query.");
        var countOption = new Option<int>("--count", () => 10);

        var rootCommand = new RootCommand {argument, categoryOption, countOption, queryOption};
        rootCommand.SetHandler<string, string, string, int>(Handler, argument, categoryOption, queryOption,
            countOption);

        rootCommand.Description = "Utility to search for news on the Meduza website.";

        await rootCommand.InvokeAsync(args);
    }

    private async Task Handler(string action, string category, string query, int newsCount)
    {
        if (action.Equals("search", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.WriteLine($"Search query is {query}");
            Console.WriteLine("Search doesn't work yet...");
            return;
        }

        var headingsData = await Methods.GetHeadingsDataModel(_httpClientFactory, "headings", category) ??
                           new HeadingsDataModel();
        var headingModels = headingsData.Documents.Select(pair => pair.Value)
            .Take(Convert.ToInt32(newsCount))
            .ToList();

        for (var i = 0; i < headingModels.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {headingModels[i].Title}");
        }
    }
}