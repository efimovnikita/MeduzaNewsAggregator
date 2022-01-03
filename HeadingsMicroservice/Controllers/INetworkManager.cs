namespace HeadingsMicroservice.Controllers;

public interface INetworkManager
{
    Task<string> GetResponse(string url);
}