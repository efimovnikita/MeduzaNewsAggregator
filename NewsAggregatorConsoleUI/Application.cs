using HeadingsMicroservice.Models;
using Newtonsoft.Json;

namespace NewsAggregatorConsoleUI;

public class Application : IApplication
{
    private readonly INetworkManager _networkManager;
    private readonly IConsoleManager _consoleManager;
    private readonly IHeadingUiFactory _headingUiFactory;

    public Application(INetworkManager networkManager, IConsoleManager consoleManager,
        IHeadingUiFactory headingUiFactory)
    {
        _networkManager = networkManager;
        _consoleManager = consoleManager;
        _headingUiFactory = headingUiFactory;
    }

    public async Task Run()
    {
        var category = _consoleManager.GetCategory();
        var count = _consoleManager.GetNewsCount();

        var responseString =
            await _networkManager.GetResponseString(
                $"https://headings-river.herokuapp.com/Headings/{category}/{count}");
        var headings = JsonConvert.DeserializeObject<IEnumerable<Heading>>(responseString)?.ToList() ??
                       new List<Heading>();

        if (headings.Count == 0)
        {
            Console.WriteLine("Failed to get news list");
            Console.ReadLine();
            return;
        }

        var uiHeadings = _headingUiFactory.GetHeadingUiList(headings);

        var newsNumber = _consoleManager.PrintHeadingsAndChooseNumber(uiHeadings);

        while (newsNumber != string.Empty)
        {
            if (!uiHeadings.Any(heading => heading.Number.Equals(newsNumber)))
            {
                return;
            }

            var headingUrl =
                uiHeadings.FirstOrDefault(representation => representation.Number.Equals(newsNumber))?.Heading.Url ??
                string.Empty;
            responseString =
                await _networkManager.GetResponseString(
                    $"https://articles-river.herokuapp.com/Articles/{headingUrl.Replace("/", "%2f")}");

            Console.WriteLine(responseString);
            Console.WriteLine();

            newsNumber = _consoleManager.GetNewsNumber();
        }
    }
}