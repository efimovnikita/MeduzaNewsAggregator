using Autofac;
using NewsAggregatorConsoleUI;

ContainerConfig.Configure().Resolve<IApplication>().Run().Wait();