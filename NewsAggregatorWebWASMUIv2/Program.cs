using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using HeadingsGRPCMicroservice;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NewsAggregatorWebWASMUIv2;
using NewsAggregatorWebWASMUIv2.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IState, AppState>();


var uri = new Uri("https://headingsgrpcmicroservice-rffu2p25hq-ey.a.run.app");


builder.Services.AddSingleton(_ => 
{ 
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress(uri, new GrpcChannelOptions { HttpClient = httpClient }); 
    return new HeadingsService.HeadingsServiceClient(channel); 
});
builder.Services.AddSingleton(_ => 
{ 
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var channel = GrpcChannel.ForAddress(uri, new GrpcChannelOptions { HttpClient = httpClient });
    return new ArticlesService.ArticlesServiceClient(channel);
});

await builder.Build().RunAsync();
