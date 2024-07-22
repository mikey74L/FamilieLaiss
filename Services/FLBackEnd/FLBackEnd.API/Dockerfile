FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/FlBackEnd/FlBackEnd.API/FlBackEnd.API.csproj", "Services/FlBackEnd/FlBackEnd.API/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["DDDHelpers/ServiceLayer/ServiceLayerHelper/ServiceLayerHelper.csproj", "DDDHelpers/ServiceLayer/ServiceLayerHelper/"]
COPY ["DDDHelpers/DTO/DTOHelper/DTOHelper.csproj", "DDDHelpers/DTO/DTOHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["FamilieLaissCoreHelpers/FamilieLaissCoreHelpers.csproj", "FamilieLaissCoreHelpers/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/FlBackEnd/FlBackEnd.Domain/FlBackEnd.Domain.csproj", "Services/FlBackEnd/FlBackEnd.Domain/"]
COPY ["Services/FlBackEnd/FlBackEnd.Infrastructure/FlBackEnd.Infrastructure.csproj", "Services/FlBackEnd/FlBackEnd.Infrastructure/"]
RUN dotnet restore "./Services/FlBackEnd/FlBackEnd.API/./FlBackEnd.API.csproj"
COPY . .
WORKDIR "/src/Services/FlBackEnd/FlBackEnd.API"
RUN dotnet build "./FlBackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlBackEnd.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlBackEnd.API.dll"]