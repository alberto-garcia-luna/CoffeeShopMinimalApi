#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoffeeShopMinimalApi/CoffeeShopMinimalApi.csproj", "CoffeeShopMinimalApi/"]
COPY ["CoffeeShopMinimalApi.Infrastructure/CoffeeShopMinimalApi.Infrastructure.csproj", "CoffeeShopMinimalApi.Infrastructure/"]
RUN dotnet restore "./CoffeeShopMinimalApi/CoffeeShopMinimalApi.csproj"
COPY . .
WORKDIR "/src/CoffeeShopMinimalApi"
RUN dotnet build "./CoffeeShopMinimalApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoffeeShopMinimalApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoffeeShopMinimalApi.dll"]