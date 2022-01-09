namespace Common.Services;

public interface INetworkService
{
    Task<string> GetResponse(string url);
    Task SendPostRequest(string url);
    Task SendPostRequestWithJsonBody<T>(string url, T model);
}