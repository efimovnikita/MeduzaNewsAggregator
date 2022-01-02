namespace NewsAggregatorConsoleUI;

public class ConsoleManager : IConsoleManager
{
    public string GetNewsCount()
    {
        Console.WriteLine("Enter the number of news:");
        var newsCount = Console.ReadLine();
        Console.WriteLine();
        return newsCount ?? string.Empty;
    }

    public string GetCategory()
    {
        Console.WriteLine("Choose category:");
        var category = Console.ReadLine();
        Console.WriteLine();
        return category ?? string.Empty;
    }

    public string GetNewsNumber()
    {
        Console.WriteLine("Enter the news number for detailed information:");
        var newsNumber = Console.ReadLine();
        Console.WriteLine();
        return newsNumber ?? string.Empty;
    }

    public void PrintHeadings(List<HeadingUi> list)
    {
        foreach (var uiHeading in list)
        {
            Console.WriteLine($"{uiHeading.Number}) {uiHeading.Heading.Title}");
        }

        Console.WriteLine();
    }

    public string PrintHeadingsAndChooseNumber(List<HeadingUi> headingUiRepresentations)
    {
        PrintHeadings(headingUiRepresentations);
        return GetNewsNumber();
    }
}