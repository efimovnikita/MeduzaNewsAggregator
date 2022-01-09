using System.Text;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using Common.Models;

namespace Common.Services;

public class AngleSharpParser : IHtmlParserService
{
    public async Task<string> ProcessArticleBody(ArticleData article)
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

        var cleanString = CleanString(body);
        var result = SplitIntoParagraphs(cleanString, 6);
        return result;
    }

    private string SplitIntoParagraphs(string text, int sentencesCountInParagraph)
    {
        char[] arrSplitChars = { '.', '?', '!' };
        var splitSentences = text.Split(arrSplitChars,StringSplitOptions.RemoveEmptyEntries);

        var sb = new StringBuilder();
        for (var i = 0; i < splitSentences.Length; i++)
        {
            sb.Append($"{splitSentences[i].Trim()}. ");
            if (i % sentencesCountInParagraph == 0)
            {
                sb.Append("\n\n");
            }
        }

        return sb.ToString().Trim();
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