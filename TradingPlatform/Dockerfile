FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE ${PORT}
ENV ASPNETCORE_URLS=http://*:${PORT}

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["TradingPlatform/TradingPlatform.csproj", "TradingPlatform/"]
RUN dotnet restore "TradingPlatform/TradingPlatform.csproj"
COPY . .
WORKDIR "/src/TradingPlatform"
RUN dotnet build "TradingPlatform.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradingPlatform.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradingPlatform.dll"]