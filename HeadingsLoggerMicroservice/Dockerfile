FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY HeadingsLoggerMicroservice.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HeadingsLoggerMicroservice.dll
