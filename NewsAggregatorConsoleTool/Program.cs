using Microsoft.Extensions.DependencyInjection;
using NewsAggregatorConsoleTool;

using var host = HostBuilder.CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;
await services.GetRequiredService<App>().Run(args);