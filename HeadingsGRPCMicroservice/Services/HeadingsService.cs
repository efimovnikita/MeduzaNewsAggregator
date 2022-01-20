using Common.HelperMethods;
using Common.Models;
using Common.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

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
        var headingsData = await Methods.GetHeadingsDataModel(_httpClientFactory, "headings", request.Category);

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
            var tuples = await Methods.GetCategoryModelTupleList(httpClient, category);

            _storageService.HeadingModels.AddRange(tuples);
        }

        var headingsDataModels = _storageService.HeadingModels
            .Where(tuple => tuple.Item2 is not null)
            .Select(tuple => tuple.Item2)
            .ToList();

        var headings = headingsDataModels
            .Select(model2 => model2!.Documents)
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

    public static Heading? FromInternalModel(HeadingModel? source)
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