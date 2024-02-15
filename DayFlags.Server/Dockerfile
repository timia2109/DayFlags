FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_HTTP_PORTS 5000
COPY build/ .
ENTRYPOINT ["dotnet", "DayFlags.Server.dll"]