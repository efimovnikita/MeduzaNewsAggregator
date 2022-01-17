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
builder.Services.AddSingleton(services => 
{ 
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var uri = new Uri("https://localhost:7055");
    var channel = GrpcChannel.ForAddress(uri, new GrpcChannelOptions { HttpClient = httpClient }); 
    return new HeadingsService.HeadingsServiceClient(channel); 
});

await builder.Build().RunAsync();
