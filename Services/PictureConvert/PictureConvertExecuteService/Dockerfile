FROM mcr.microsoft.com/dotnet/aspnet:8.0.2 AS base

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0.201 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/PictureConvert/PictureConvertExecuteService/PictureConvertExecuteService.csproj", "Services/PictureConvert/PictureConvertExecuteService/"]
COPY ["DDDHelpers/ExecuterLayer/ExecuterLayerHelper/ExecuterLayerHelper.csproj", "DDDHelpers/ExecuterLayer/ExecuterLayerHelper/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/PictureConvert/PictureConvert.Domain/PictureConvert.Domain.csproj", "Services/PictureConvert/PictureConvert.Domain/"]
COPY ["Services/PictureConvert/PictureConvert.Infrastructure/PictureConvert.Infrastructure.csproj", "Services/PictureConvert/PictureConvert.Infrastructure/"]
RUN dotnet restore "./Services/PictureConvert/PictureConvertExecuteService/./PictureConvertExecuteService.csproj"
COPY . .
WORKDIR "/src/Services/PictureConvert/PictureConvertExecuteService"
RUN dotnet build "./PictureConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PictureConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PictureConvertExecuteService.dll"]