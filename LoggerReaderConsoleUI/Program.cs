using Newtonsoft.Json;

async Task<List<string>> GetResponseString(string url)
{
    var client = new HttpClient();
    var builder = new UriBuilder(url);
    var responseMessage = await client.GetAsync(builder.Uri);
    
    if (!responseMessage.IsSuccessStatusCode)
    {
        Console.WriteLine("Response unsuccessful");
        return Array.Empty<string>().ToList();
    }

    var responseString = await responseMessage.Content.ReadAsStringAsync();
    var responseList = JsonConvert.DeserializeObject<List<string>>(responseString) ?? Array.Empty<string>().ToList();
    return responseList;
}

var list = await GetResponseString("https://logs-river.herokuapp.com/Logs");
var groups = list.GroupBy(s => s).ToList();
var groupingHeadings = groups
    .OrderByDescending(grouping => grouping.Count())
    .Select(grouping => grouping.Key).ToList();

Console.WriteLine("Headings log:");
Console.WriteLine();

for (var i = 0; i < groupingHeadings.Count; i++)
{
    Console.WriteLine($"{i + 1}) {groupingHeadings[i]}");
}

Console.ReadLine();