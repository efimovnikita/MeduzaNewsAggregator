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

#if RELEASE
    var uri = new Uri("http://20.113.15.205:80");
#endif
#if DEBUG
    var uri = new Uri("https://localhost:7055");
#endif

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
