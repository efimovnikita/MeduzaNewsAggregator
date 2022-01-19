FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./publish /publish
WORKDIR /publish
ENTRYPOINT ["dotnet", "HeadingsGRPCMicroservice.dll"]