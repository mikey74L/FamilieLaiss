# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/PictureConvert/PictureConvertExecuteService/PictureConvertExecuteService.csproj", "Services/PictureConvert/PictureConvertExecuteService/"]
COPY ["DDDHelpers/ExecuterLayer/ExecuterLayerHelper/ExecuterLayerHelper.csproj", "DDDHelpers/ExecuterLayer/ExecuterLayerHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
RUN dotnet restore "./Services/PictureConvert/PictureConvertExecuteService/PictureConvertExecuteService.csproj"
COPY . .
WORKDIR "/src/Services/PictureConvert/PictureConvertExecuteService"
RUN dotnet build "./PictureConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PictureConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PictureConvertExecuteService.dll"]