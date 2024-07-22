#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/VideoConvert/VideoConvert.API/VideoConvert.API.csproj", "Services/VideoConvert/VideoConvert.API/"]
COPY ["DDDHelpers/ServiceLayer/ServiceLayerHelper/ServiceLayerHelper.csproj", "DDDHelpers/ServiceLayer/ServiceLayerHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["DDDHelpers/DTO/DTOHelper/DTOHelper.csproj", "DDDHelpers/DTO/DTOHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["FamilieLaissCoreHelpers/FamilieLaissCoreHelpers.csproj", "FamilieLaissCoreHelpers/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/VideoConvert/VideoConvert.Domain/VideoConvert.Domain.csproj", "Services/VideoConvert/VideoConvert.Domain/"]
COPY ["Services/VideoConvert/VideoConvert.Infrastructure/VideoConvert.Infrastructure.csproj", "Services/VideoConvert/VideoConvert.Infrastructure/"]
RUN dotnet restore "./Services/VideoConvert/VideoConvert.API/VideoConvert.API.csproj"
COPY . .
WORKDIR "/src/Services/VideoConvert/VideoConvert.API"
RUN dotnet build "./VideoConvert.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoConvert.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoConvert.API.dll"]