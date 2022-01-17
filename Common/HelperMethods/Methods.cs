using Common.Models;

namespace Common.HelperMethods;

public static class Methods
{
    public static IEnumerable<HeadingModel2> GetHeadingsList2(string category, HeadingsDataModel2? headings)
    {
        var documents = headings?.Documents
            .Where(pair => pair.Value.Tag.Name.Equals(GetTagNameBasedOnCategory(category)))
            .Select(pair => pair.Value);

        documents = documents?
            .Where(model => model.Document_Type.Equals("video") == false)
            .OrderByDescending(model => model.Pub_Date)
            .ThenByDescending(model => model.Published_At);

        return documents ?? Array.Empty<HeadingModel2>();
    }

    public static IEnumerable<HeadingModel> GetHeadingsList(string category, HeadingsDataModel? headings)
    {
        var documents = headings?.Documents
            .Where(pair =>
            {
                var headingModel = pair.Value;
                headingModel.Tag.TryGetValue("name", out var nameTagValue);
                return string.IsNullOrWhiteSpace(nameTagValue) != true &&
                       nameTagValue.Equals(GetTagNameBasedOnCategory(category),
                           StringComparison.InvariantCultureIgnoreCase);
            })
            .Select(pair => pair.Value);

        documents = documents?
            .Where(model => model.Document_Type.Equals("video") == false)
            .OrderByDescending(model => model.Pub_Date)
            .ThenByDescending(model => model.Published_At);

        return documents ?? Array.Empty<HeadingModel>();
    }

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
}