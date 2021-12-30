using HeadingsMicroservice.Models;
using Newtonsoft.Json;

string? GetNewsCount()
{
    Console.WriteLine("Enter the number of news:");
    var count1 = Console.ReadLine();
    Console.WriteLine();
    return count1;
}

string? GetCategory()
{
    Console.WriteLine("Choose category:");
    var s = Console.ReadLine();
    Console.WriteLine();
    return s;
}

string? GetNewsNumber()
{
    Console.WriteLine("Enter the news number for detailed information:");
    var number = Console.ReadLine();
    Console.WriteLine();
    return number;
}

void PrintHeadings(List<HeadingUiRepresentation> list)
{
    foreach (var uiHeading in list)
    {
        Console.WriteLine($"{uiHeading.Number}) {uiHeading.Heading.Title}");
    }

    Console.WriteLine();
}

string? PrintHeadingsAndChooseNumber(List<HeadingUiRepresentation> headingUiRepresentations)
{
    PrintHeadings(headingUiRepresentations);

    return GetNewsNumber();
}

async Task<string> GetResponseString(string url)
{
    var client = new HttpClient();
    var builder = new UriBuilder(url);
    var responseMessage = await client.GetAsync(builder.Uri);
    
    if (!responseMessage.IsSuccessStatusCode)
    {
        Console.WriteLine("Response unsuccessful");
        return string.Empty;
    }

    var responseString = await responseMessage.Content.ReadAsStringAsync();
    return responseString;
}

//const string baseUrl = "http://localhost";


var category = GetCategory();
var count = GetNewsCount();

var responseString = await GetResponseString($"https://headings-river.herokuapp.com/Headings/{category}/{count}");
var headings = JsonConvert.DeserializeObject<IEnumerable<Heading>>(responseString)?.ToList() ?? new List<Heading>();

if (headings.Count == 0)
{
    Console.WriteLine("Failed to get news list");
    Console.ReadLine();
    return;
}

var i = 1;
var uiHeadings = headings
    .OrderByDescending(heading => heading.Pub_Date)
    .Select(heading =>
    {
        var uiRepresentation = new HeadingUiRepresentation(i.ToString(), heading);
        i++;
        return uiRepresentation;
    }).ToList();

var newsNumber = PrintHeadingsAndChooseNumber(uiHeadings);

while (newsNumber is not null)
{
    if (!uiHeadings.Any(heading => heading.Number.Equals(newsNumber)))
    {
        return;
    }

    var headingUrl = uiHeadings.FirstOrDefault(representation => representation.Number.Equals(newsNumber))?.Heading.Url ?? string.Empty;
    responseString = await GetResponseString($"https://articles-river.herokuapp.com/Articles/{headingUrl.Replace("/", "%2f")}");

    Console.WriteLine(responseString);
    Console.WriteLine();

    newsNumber = GetNewsNumber();
}
