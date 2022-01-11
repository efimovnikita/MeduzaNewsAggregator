FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./publish /publish
WORKDIR /publish
CMD ASPNETCORE_URLS=http://*:$PORT dotnet SearchMicroservice.dll
