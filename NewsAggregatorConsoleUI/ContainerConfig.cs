using Autofac;
using LoggerReaderConsoleUI;

namespace NewsAggregatorConsoleUI;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Application>().As<IApplication>();
        builder.RegisterType<NetworkManager>().As<INetworkManager>();
        builder.RegisterType<HttpClient>().AsSelf();
        builder.RegisterType<UriBuilderFactory>().As<IUriBuilderFactory>();
        builder.RegisterType<HeadingUiFactory>().As<IHeadingUiFactory>();
        builder.RegisterType<ConsoleManager>().As<IConsoleManager>();

        return builder.Build();
    }
}