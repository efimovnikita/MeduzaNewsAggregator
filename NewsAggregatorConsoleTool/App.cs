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
            var httpClient = _httpClientFactory.CreateClient("headings");
            var tuples = await Methods.GetCategoryModelTupleList(httpClient, category);

            var dataModels = tuples.Where(tuple => tuple.model is not null)
                .Where(tuple => tuple.category.Equals(category))
                .Select(tuple => tuple.model)
                .ToList();

            var models = dataModels.Select(model => model!.Documents)
                .SelectMany(models => models.Values)
                .Where(model => model.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                .Distinct()
                .ToList();

            Print(models);
            
            return;
        }

        var headingsData = await Methods.GetHeadingsDataModel(_httpClientFactory, "headings", category) ??
                           new HeadingsDataModel();
        var headingModels = headingsData.Documents.Select(pair => pair.Value)
            .Take(Convert.ToInt32(newsCount))
            .ToList();

        Print(headingModels);
    }

    private static void Print(IReadOnlyList<HeadingModel> headingModels)
    {
        for (var i = 0; i < headingModels.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {headingModels[i].Title}");
        }
    }
}