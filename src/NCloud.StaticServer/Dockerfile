#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-alpine3.13-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 21
EXPOSE 10000-10099
EXPOSE 11111

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/NCloud.StaticServer/NCloud.StaticServer.csproj", "src/NCloud.StaticServer/"]
RUN dotnet restore "src/NCloud.StaticServer/NCloud.StaticServer.csproj"
COPY . .
WORKDIR "/src/src/NCloud.StaticServer"
RUN dotnet build "NCloud.StaticServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NCloud.StaticServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NCloud.StaticServer.dll"]