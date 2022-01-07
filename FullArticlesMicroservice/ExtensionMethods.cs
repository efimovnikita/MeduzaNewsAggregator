using FullArticlesMicroservice.Models;
using FullArticlesMicroservice.Services;
using HeadingsMicroservice.Services;

namespace FullArticlesMicroservice;

public static class ExtensionMethods
{
    public static IServiceCollection AddMicroserviceDependencies(this IServiceCollection services)
    {
        services.Add(ServiceDescriptor.Transient<IHtmlParserService, AngleSharpParser>());
        services.Add(ServiceDescriptor.Transient<INetworkService, RestSharpNetworkService>());
        return services;
    }
}