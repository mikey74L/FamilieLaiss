# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/Settings/Settings.API/Settings.API.csproj", "Services/Settings/Settings.API/"]
COPY ["DDDHelpers/ServiceLayer/ServiceLayerHelper/ServiceLayerHelper.csproj", "DDDHelpers/ServiceLayer/ServiceLayerHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["DDDHelpers/DTO/DTOHelper/DTOHelper.csproj", "DDDHelpers/DTO/DTOHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["FamilieLaissCoreHelpers/FamilieLaissCoreHelpers.csproj", "FamilieLaissCoreHelpers/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/Settings/Settings.Domain/Settings.Domain.csproj", "Services/Settings/Settings.Domain/"]
COPY ["Services/Settings/Settings.Infrastructure/Settings.Infrastructure.csproj", "Services/Settings/Settings.Infrastructure/"]
RUN dotnet restore "./Services/Settings/Settings.API/Settings.API.csproj"
COPY . .
WORKDIR "/src/Services/Settings/Settings.API"
RUN dotnet build "./Settings.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Settings.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Settings.API.dll"]