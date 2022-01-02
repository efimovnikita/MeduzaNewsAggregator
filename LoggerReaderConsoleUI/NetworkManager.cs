using Newtonsoft.Json;

namespace LoggerReaderConsoleUI;

public class NetworkManager : INetworkManager
{
    private readonly HttpClient _httpClient;
    private readonly IUriBuilderFactory _builderFactory;

    public NetworkManager(IUriBuilderFactory builderFactory)
    {
        _builderFactory = builderFactory;
        _httpClient = new HttpClient();
    }

    public async Task<List<string>> GetResponseStringList(string url)
    {
        var builder = _builderFactory.GetBuilder(url);
        var responseMessage = await _httpClient.GetAsync(builder.Uri);
    
        if (!responseMessage.IsSuccessStatusCode)
        {
            Console.WriteLine("Response unsuccessful");
            return Array.Empty<string>().ToList();
        }

        var responseString = await responseMessage.Content.ReadAsStringAsync();
        var responseList = JsonConvert.DeserializeObject<List<string>>(responseString) ?? Array.Empty<string>().ToList();
        return responseList;
    }
}