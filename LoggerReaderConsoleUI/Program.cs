using Autofac;
using LoggerReaderConsoleUI;

var container = ContainerConfig.Configure();
using var scope = container.BeginLifetimeScope();
var application = scope.Resolve<IApplication>();
application.Run().Wait();