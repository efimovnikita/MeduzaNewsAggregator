using Autofac;

namespace LoggerReaderConsoleUI;

public static class ContainerConfig
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<Application>().As<IApplication>();
        builder.RegisterType<NetworkManager>().As<INetworkManager>().SingleInstance();
        builder.RegisterType<HttpClient>().AsSelf();
        builder.RegisterType<UriBuilderFactory>().As<IUriBuilderFactory>();

        return builder.Build();
    }
}