using System.Net;
using HeadingsSignalRMicroservice;
using Timer = System.Timers.Timer;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddCors();
builder.Services.AddHttpClient("headings")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
builder.Services.AddTransient(_ => new Timer(3600000));
var app = builder.Build();

app.MapHub<MainHub>("/headings");

app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyHeader());

app.Run();
