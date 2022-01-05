
using FullArticlesMicroservice.Controllers;
using HeadingsMicroservice.Controllers;

namespace FullArticlesMicroservice;

public static class ExtensionMethods
{
    public static IServiceCollection AddMicroserviceDependencies(this IServiceCollection services)
    {
        services.Add(ServiceDescriptor.Transient<IHtmlParser, HtmlParser>());
        services.Add(ServiceDescriptor.Transient<INetworkService, HttpClientNetworkService>());
        return services;
    }
}