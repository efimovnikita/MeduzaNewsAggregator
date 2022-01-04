using System.IO.Compression;

namespace HeadingsMicroservice.Controllers;

public class NetworkManager : INetworkManager
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
}