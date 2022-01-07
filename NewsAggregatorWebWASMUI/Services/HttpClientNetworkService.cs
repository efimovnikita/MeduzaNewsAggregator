using System.IO.Compression;

namespace NewsAggregatorWebWASMUI.Services;

public class HttpClientNetworkService : INetworkService
{
    public async Task<string> GetResponse(string url)
    {
        var client = new HttpClient();
        var stringAsync = await client.GetStringAsync(url);
        return await new StringReader(stringAsync).ReadToEndAsync();
    }

    public async Task SendPostRequest(string url)
    {
        var client = new HttpClient();
        await client.PostAsync(url, null);
    }

    public Task SendPostRequestWithJsonBody<T>(string url, T model)
    {
        throw new NotImplementedException();
    }
}