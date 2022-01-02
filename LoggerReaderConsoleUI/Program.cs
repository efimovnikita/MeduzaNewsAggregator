using Autofac;
using LoggerReaderConsoleUI;

ContainerConfig.Configure().Resolve<IApplication>().Run();

Console.ReadLine();