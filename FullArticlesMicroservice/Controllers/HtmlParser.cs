using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using FullArticlesMicroservice.Models;

namespace FullArticlesMicroservice.Controllers;

public class HtmlParser : IHtmlParser
{
    public async Task<string> ProcessArticleBody(Data article)
    {
        var contentBody = article.Root.Content.Body;

        var config = Configuration.Default.WithJs();
        var context = BrowsingContext.New(config);

        var document = await context.OpenAsync(req => { req.Content(contentBody); });
        var body = document.Body;
        if (body is null)
        {
            return string.Empty;
        }

        RemoveScriptTags(document);

        var result = CleanString(body);
        return result;
    }

    private string CleanString(INode body)
    {
        var bodyText = body.Text().Trim();
        var removedEmptyLinesString = Regex.Replace(bodyText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        
        var linesArray = Regex.Split(removedEmptyLinesString, "\r\n|\r|\n");
        var resultLinesArray = linesArray.Select(line => line.Trim()).ToList();

        return string.Join('\n', resultLinesArray);
    }

    private void RemoveScriptTags(IParentNode document)
    {
        var scriptTags = document.QuerySelectorAll("script");
        foreach (var tag in scriptTags)
        {
            tag.Remove();
        }
    }
}