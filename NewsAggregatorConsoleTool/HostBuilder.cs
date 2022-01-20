using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NewsAggregatorConsoleTool;

public static class HostBuilder
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder => builder.ClearProviders())
            .ConfigureServices((_, services) =>
            {
                services
                    .AddHttpClient("headings")
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });

                services.AddTransient<App>();
            });
}