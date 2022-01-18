using Common.Models;
using Common.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;

namespace HeadingsGRPCMicroservice.Services;

public class HeadingsService : HeadingsGRPCMicroservice.HeadingsService.HeadingsServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStorageService _storageService;

    public HeadingsService(IHttpClientFactory httpClientFactory, IStorageService storageService)
    {
        _httpClientFactory = httpClientFactory;
        _storageService = storageService;
    }

    public override async Task<HeadingsResponse> GetHeadings(GetHeadingsRequest request, ServerCallContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var responseMessage = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            $"https://meduza.io/api/v3/search?chrono={request.Category}&locale=ru&page=0&per_page=24"));

        if (responseMessage.IsSuccessStatusCode == false)
        {
            return new HeadingsResponse
            {
                Headings = { new List<Heading>() }
            };
        }

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel2>(contentString);

        var headings = headingsData!.Documents
            .Select(pair => pair.Value)
            .Select(FromInternalModel)
            .ToList();
        return new HeadingsResponse
        {
            Headings = { headings }
        };
    }
    
    public override async Task<HeadingsResponse> SearchHeadings(SearchRequest searchRequest, ServerCallContext context)
    {
        var httpClient = _httpClientFactory.CreateClient("headings");
        var category = searchRequest.Category;
        
        var savedCategoryTuples = _storageService.HeadingModels
            .Where(tuple => tuple.Item1.Equals(category))
            .ToList();
        
        if (savedCategoryTuples.Count == 0)
        {
            for (var i = 0; i < 150; i++)
            {
                var headingsData = await GetSingleHeadingsData(httpClient, category, i);
                if (headingsData == null) continue;
                _storageService.HeadingModels.Add((category, headingsData));
            }
        }

        var headingsDataModels = _storageService.HeadingModels.Select(tuple => tuple.Item2).ToList();

        var headings = headingsDataModels
            .Select(model2 => model2.Documents)
            .SelectMany(model2S => model2S.Values)
            .Select(FromInternalModel)
            .ToList();

        var filteredHeadings = headings
            .Where(heading => heading!.Title.Contains(searchRequest.Searchquery, StringComparison.InvariantCultureIgnoreCase))
            .Distinct()
            .ToList();

        return new HeadingsResponse
        {
            Headings = { filteredHeadings }
        };
    }

    private static async Task<HeadingsDataModel2?> GetSingleHeadingsData(HttpClient client, string category, int page)
    {
        var responseMessage = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            $"https://meduza.io/api/v3/search?chrono={category}&locale=ru&page={page}&per_page=100"));

        if (responseMessage.IsSuccessStatusCode == false)
        {
            return null;
        }

        var contentString = await responseMessage.Content.ReadAsStringAsync();
        var headingsData = JsonConvert.DeserializeObject<HeadingsDataModel2>(contentString);
        return headingsData;
    }

    public static Heading? FromInternalModel(HeadingModel2? source)
    {
        if (source is null)
        {
            return null;
        }

        source.Tag.TryGetValue("name", out var nameTag);
        var tag = new Tag {Name = nameTag ?? String.Empty, Path = String.Empty};

        return new Heading
        {
            Title = source.Title,
            Pubdate = Timestamp.FromDateTimeOffset(source.Pub_Date),
            Publishedat = source.Published_At,
            Tag = tag,
            Type = source.Document_Type,
            Url = source.Url
        };
    }
}