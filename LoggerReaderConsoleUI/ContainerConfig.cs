using Autofac;

namespace LoggerReaderConsoleUI;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.Register(context => new Application(context.Resolve<INetworkManager>()))
            .As<IApplication>();
        builder.Register(context => new NetworkManager(context.Resolve<IUriBuilderFactory>()))
            .As<INetworkManager>();
        builder.Register(_ => new HttpClient()).AsSelf();
        builder.Register(_ => new UriBuilderFactory()).As<IUriBuilderFactory>();

        return builder.Build();
    }
}