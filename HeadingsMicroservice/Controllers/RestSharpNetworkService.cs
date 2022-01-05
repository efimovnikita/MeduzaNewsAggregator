using RestSharp;

namespace HeadingsMicroservice.Controllers;

public class RestSharpNetworkService : INetworkService
{
    public async Task<string> GetResponse(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest();
        var response = await client.GetAsync<string>(request);

        return response;
    }

    public async Task SendPostRequest(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest();
        await client.PostAsync<Task>(request);
    }
}