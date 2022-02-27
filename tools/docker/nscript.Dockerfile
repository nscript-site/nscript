FROM mcr.microsoft.com/dotnet/aspnet:6.0.2-alpine3.14-amd64
WORKDIR /app
ENTRYPOINT ["dotnet", "Server.dll"]
