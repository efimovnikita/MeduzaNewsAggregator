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
        var category = string.Empty;
        var newsCount = string.Empty;

        switch (args.Length)
        {
            case 0:
                Console.Write("Category: ");
                category = Console.ReadLine()!.Trim();
                newsCount = "10";
                break;
            case 1:
                category = args[0];
                newsCount = "10";
                break;
            case 2:
                category = args[0];
                newsCount = args[1];
                break;
        }

        var headingsData = await Methods.GetHeadingsDataModel(_httpClientFactory, "headings", category) ?? new HeadingsDataModel();
        var headingModels = headingsData.Documents
            .Select(pair => pair.Value)
            .Take(Convert.ToInt32(newsCount))
            .ToList();

        for (var i = 0; i < headingModels.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {headingModels[i].Title}");
        }

        Console.ReadLine();
    }
}