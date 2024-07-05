FROM mcr.microsoft.com/dotnet/aspnet:8.0.2 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0.201 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/Scheduler/Scheduler.API/Scheduler.API.csproj", "Services/Scheduler/Scheduler.API/"]
COPY ["DDDHelpers/ServiceLayer/ServiceLayerHelper/ServiceLayerHelper.csproj", "DDDHelpers/ServiceLayer/ServiceLayerHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["DDDHelpers/DTO/DTOHelper/DTOHelper.csproj", "DDDHelpers/DTO/DTOHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["FamilieLaissCoreHelpers/FamilieLaissCoreHelpers.csproj", "FamilieLaissCoreHelpers/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/Scheduler/Scheduler.Domain/Scheduler.Domain.csproj", "Services/Scheduler/Scheduler.Domain/"]
COPY ["Services/Scheduler/Scheduler.DTO/Scheduler.DTO.csproj", "Services/Scheduler/Scheduler.DTO/"]
COPY ["Services/Scheduler/Scheduler.Infrastructure/Scheduler.Infrastructure.csproj", "Services/Scheduler/Scheduler.Infrastructure/"]
RUN dotnet restore "./Services/Scheduler/Scheduler.API/./Scheduler.API.csproj"
COPY . .
WORKDIR "/src/Services/Scheduler/Scheduler.API"
RUN dotnet build "./Scheduler.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Scheduler.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scheduler.API.dll"]