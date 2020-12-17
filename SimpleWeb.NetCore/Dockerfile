#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build runtime image

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim

WORKDIR /app

COPY ./out .

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "SimpleWeb.NetCore.dll"]