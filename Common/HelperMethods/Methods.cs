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

    public static async Task<IEnumerable<(string category, HeadingsDataModel? model)>> GetCategoryModelTupleList(
        HttpClient httpClient, string category)
    {
        var responseMessagesTasks = GetResponseMessagesTasks(httpClient, category);
        var responseMessages = await Task.WhenAll(responseMessagesTasks);

        var contentStringTasks = responseMessages.Select(responseMessage => responseMessage.Content.ReadAsStringAsync())
            .ToList();

        var contentStrings = await Task.WhenAll(contentStringTasks);

        var deserializedModelsTasks = contentStrings
            .Select(content => Task.Run(() => DeserializeHeadingsData(content)))
            .ToList();

        var models = await Task.WhenAll(deserializedModelsTasks);
        return ZipCategoriesWithModels(models, category);
    }

    private static IEnumerable<(string category, HeadingsDataModel? model)> ZipCategoriesWithModels(
        IReadOnlyCollection<HeadingsDataModel?> models, string category)
    {
        List<string> categories = new();
        for (var i = 0; i < models.Count; i++)
        {
            categories.Add(category);
        }

        IEnumerable<(string category, HeadingsDataModel? model)> tuples = categories.Zip(models);
        return tuples;
    }

    private static HeadingsDataModel? DeserializeHeadingsData(string content)
    {
        HeadingsDataModel? headingsData = default;
        try
        {
            headingsData = JsonConvert.DeserializeObject<HeadingsDataModel>(content);
        }
        catch
        {
            // ignored
        }

        return headingsData;
    }

    private static IEnumerable<Task<HttpResponseMessage>> GetResponseMessagesTasks(HttpClient httpClient,
        string category)
    {
        List<Task<HttpResponseMessage>> responseMessagesTasks = new();
        for (var i = 0; i < 150; i++)
        {
            responseMessagesTasks.Add(httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page={i}&per_page=100")));
        }

        return responseMessagesTasks;
    }
}