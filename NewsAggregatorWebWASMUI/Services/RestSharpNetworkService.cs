using RestSharp;

namespace NewsAggregatorWebWASMUI.Services;

public class RestSharpNetworkService : INetworkService
{
    public async Task<string> GetResponse(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest();
        var response = await client.ExecuteGetAsync<string>(request);

        return response.Content;
    }

    public async Task SendPostRequest(string url)
    {
        var client = new RestClient(url);
        var request = new RestRequest();
        await client.ExecutePostAsync(request);
    }

    public async Task SendPostRequestWithJsonBody<T>(string url, T model)
    {
        if (model == null)
        {
            throw new NullReferenceException("Model can not be null");
        }
        
        var client = new RestClient(url);
        var request = new RestRequest
        {
            RequestFormat = DataFormat.Json
        };
        request.AddJsonBody(model);
        await client.ExecutePostAsync(request);
    }
}