using LoggerReaderConsoleUI;

namespace NewsAggregatorConsoleUI;

public class NetworkManager : INetworkManager
{
    private readonly HttpClient _httpClient;
    private readonly IUriBuilderFactory _builderFactory;

    public NetworkManager(IUriBuilderFactory builderFactory)
    {
        _builderFactory = builderFactory;
        _httpClient = new HttpClient();
    }
    
    public async Task<string> GetResponseString(string url)
    {
        var builder = _builderFactory.GetBuilder(url);
        var responseMessage = await _httpClient.GetAsync(builder.Uri);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadAsStringAsync();
        }

        Console.WriteLine("Response unsuccessful");
        return string.Empty;
    }
}