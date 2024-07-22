#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/PictureConvert/PictureConvert.API/PictureConvert.API.csproj", "Services/PictureConvert/PictureConvert.API/"]
COPY ["DDDHelpers/ServiceLayer/ServiceLayerHelper/ServiceLayerHelper.csproj", "DDDHelpers/ServiceLayer/ServiceLayerHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["DDDHelpers/DTO/DTOHelper/DTOHelper.csproj", "DDDHelpers/DTO/DTOHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["FamilieLaissCoreHelpers/FamilieLaissCoreHelpers.csproj", "FamilieLaissCoreHelpers/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/PictureConvert/PictureConvert.Infrastructure/PictureConvert.Infrastructure.csproj", "Services/PictureConvert/PictureConvert.Infrastructure/"]
COPY ["Services/PictureConvert/PictureConvert.Domain/PictureConvert.Domain.csproj", "Services/PictureConvert/PictureConvert.Domain/"]
RUN dotnet restore "./Services/PictureConvert/PictureConvert.API/PictureConvert.API.csproj"
COPY . .
WORKDIR "/src/Services/PictureConvert/PictureConvert.API"
RUN dotnet build "./PictureConvert.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PictureConvert.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PictureConvert.API.dll"]