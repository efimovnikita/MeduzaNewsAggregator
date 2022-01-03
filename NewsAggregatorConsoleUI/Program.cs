using Autofac;
using NewsAggregatorConsoleUI;

var container = ContainerConfig.Configure();
using var scope = container.BeginLifetimeScope();
var application = scope.Resolve<IApplication>();
application.Run().Wait();