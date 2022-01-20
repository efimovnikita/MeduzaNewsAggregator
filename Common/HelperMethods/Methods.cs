using Common.Models;
using Newtonsoft.Json;

namespace Common.HelperMethods;

public static class Methods
{
    public static string GetTagNameBasedOnCategory(string category)
    {
        return category switch
        {
            "news" => "новости",
            "shapito" => "шапито",
            "articles" => "истории",
            _ => "новости"
        };
    }

    public static async Task<HeadingsDataModel?> GetHeadingsDataModel(IHttpClientFactory httpClientFactory,
        string clientName, string category)
    {
        var httpClient = httpClientFactory.CreateClient(clientName);
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page=0&per_page=24"));

        if (responseMessage.IsSuccessStatusCode == false)
        {
            return new HeadingsDataModel();
        }

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel>(contentString);
        return headingsData;
    }
}