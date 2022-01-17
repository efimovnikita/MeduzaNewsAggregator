using Common.HelperMethods;
using Common.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;

namespace HeadingsGRPCMicroservice.Services;

public class HeadingsService : HeadingsGRPCMicroservice.HeadingsService.HeadingsServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HeadingsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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

        var headingsList = Methods.GetHeadingsList2(request.Category, headingsData).ToList();
        var headings = headingsList.Select(FromInternalModel).ToList();
        return new HeadingsResponse
        {
            Headings = { headings }
        };
    }

    private static Heading? FromInternalModel(HeadingModel2? source)
    {
        if (source is null)
        {
            return null;
        }

        var tag = new Tag {Name = source.Tag.Name, Path = source.Tag.Path};

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