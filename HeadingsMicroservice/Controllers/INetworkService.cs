namespace HeadingsMicroservice.Controllers;

public interface INetworkService
{
    Task<string> GetResponse(string url);
    Task SendPostRequest(string url);
}