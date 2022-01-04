namespace HeadingsMicroservice.Controllers;

public interface INetworkManager
{
    Task<string> GetResponse(string url);
    Task SendPostRequest(string url);
}