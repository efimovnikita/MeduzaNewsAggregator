using System.Net;
using Common.Services;
using HeadingsGRPCMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddCors();
builder.Services.AddHttpClient("headings")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
builder.Services.AddTransient<IHtmlParserService, AngleSharpParser>();
builder.Services.AddSingleton<IStorageService, HeadingsStorageService>();

var port = Environment.GetEnvironmentVariable("PORT");

builder.WebHost.ConfigureKestrel(options => options.Listen(IPAddress.Any, Convert.ToInt32(port)));

var app = builder.Build();

app.UseCors(cors => cors
    .WithOrigins("https://localhost:7149")
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseGrpcWeb();
app.MapGrpcService<HeadingsService>().EnableGrpcWeb();
app.MapGrpcService<ArticlesService>().EnableGrpcWeb();

app.Run();
