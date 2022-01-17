using System.Net;
using Common.Services;
using HeadingsGRPCMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddCors();
builder.Services.AddHttpClient("headings")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
builder.Services.AddTransient<IHtmlParserService, AngleSharpParser>();

var app = builder.Build();

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.UseGrpcWeb();
app.MapGrpcService<HeadingsService>().EnableGrpcWeb();
app.MapGrpcService<ArticlesService>().EnableGrpcWeb();

app.Run();
