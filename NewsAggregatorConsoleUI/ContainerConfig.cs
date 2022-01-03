using Autofac;
using LoggerReaderConsoleUI;

namespace NewsAggregatorConsoleUI;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.Register(context => new Application(context.Resolve<INetworkManager>(),
                context.Resolve<IConsoleManager>(),
                context.Resolve<IHeadingUiFactory>()))
            .As<IApplication>();
        builder.Register(context => new NetworkManager(context.Resolve<IUriBuilderFactory>()))
            .As<INetworkManager>();
        builder.Register(_ => new HttpClient()).AsSelf();
        builder.Register(_ => new UriBuilderFactory()).As<IUriBuilderFactory>();
        builder.Register(_ => new HeadingUiFactory()).As<IHeadingUiFactory>();
        builder.Register(_ => new ConsoleManager()).As<IConsoleManager>();

        return builder.Build();
    }
}