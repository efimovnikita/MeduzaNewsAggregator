using System.IO.Compression;

namespace HeadingsMicroservice.Services;

public class HttpClientNetworkService : INetworkService
{
    public async Task<string> GetResponse(string url)
    {
        var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(url);
        return await new StreamReader(new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            .ReadToEndAsync();
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