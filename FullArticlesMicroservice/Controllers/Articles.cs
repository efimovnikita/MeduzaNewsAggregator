using System.IO.Compression;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using FullArticlesMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Grpc.Net.Client;

namespace FullArticlesMicroservice.Controllers;

[ApiController]
[Route("[controller]")]
public class Articles
{
    [HttpGet("{url}")]
    public async Task<string> Get(string url)
    {
        var article = await GetArticle(url);
        return article;
    }

    private static async Task<string> GetArticle(string url)
    {
        var uri = $"https://meduza.io/api/v3/{url}";
        var httpClient = new HttpClient();
        var bytes = await httpClient.GetByteArrayAsync(uri);
        var json = await new StreamReader(new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
            .ReadToEndAsync();
        var article = JsonConvert.DeserializeObject<Data>(json);
        
        if (article is null)
        {
            return string.Empty;
        }
        
        // send message to logger service (GRPC not available)
        await httpClient.PostAsync($"https://logs-river.herokuapp.com/Logs?item={article.Root.Title}", null);
        // var httpHandler = new HttpClientHandler();
        // httpHandler.ServerCertificateCustomValidationCallback = 
        //     HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        // using var channel = GrpcChannel.ForAddress("https://logs-river.herokuapp.com", new GrpcChannelOptions { HttpHandler = httpHandler});
        // var client = new HeadingsLogService.HeadingsLogServiceClient(channel);
        // await client.MessageReceivedAsync(new LogRequest { Title = article.Root.Title });

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

        var result = GetCleanString(body);

        return result;
    }

    private static string GetCleanString(INode body)
    {
        var bodyText = body.Text().Trim();
        var removedEmptyLinesString = Regex.Replace(bodyText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        
        var linesArray = Regex.Split(removedEmptyLinesString, "\r\n|\r|\n");
        var resultLinesArray = linesArray.Select(line => line.Trim()).ToList();

        return string.Join('\n', resultLinesArray);
    }

    private static void RemoveScriptTags(IParentNode document)
    {
        var scriptTags = document.QuerySelectorAll("script");
        foreach (var tag in scriptTags)
        {
            tag.Remove();
        }
    }
}