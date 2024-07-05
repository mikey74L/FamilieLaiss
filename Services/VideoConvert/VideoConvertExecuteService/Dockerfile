FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-jammy AS base

RUN apt-get update \ 
    && apt-get install -y --allow-unauthenticated \
        ffmpeg \
        imagemagick \
		mediainfo \
        gpac

USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0.201 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Services/VideoConvert/VideoConvertExecuteService/VideoConvertExecuteService.csproj", "Services/VideoConvert/VideoConvertExecuteService/"]
COPY ["DDDHelpers/ExecuterLayer/ExecuterLayerHelper/ExecuterLayerHelper.csproj", "DDDHelpers/ExecuterLayer/ExecuterLayerHelper/"]
COPY ["FamilieLaissMassTransitDefinitions/FamilieLaissMassTransitDefinitions.csproj", "FamilieLaissMassTransitDefinitions/"]
COPY ["FamilieLaissSharedObjects/FamilieLaissSharedObjects.csproj", "FamilieLaissSharedObjects/"]
COPY ["Services/VideoConvert/VideoConvert.Infrastructure/VideoConvert.Infrastructure.csproj", "Services/VideoConvert/VideoConvert.Infrastructure/"]
COPY ["DDDHelpers/Infrastructure/InfrastructureHelper/InfrastructureHelper.csproj", "DDDHelpers/Infrastructure/InfrastructureHelper/"]
COPY ["DDDHelpers/Domain/DomainHelper/DomainHelper.csproj", "DDDHelpers/Domain/DomainHelper/"]
COPY ["Services/VideoConvert/VideoConvert.Domain/VideoConvert.Domain.csproj", "Services/VideoConvert/VideoConvert.Domain/"]
RUN dotnet restore "./Services/VideoConvert/VideoConvertExecuteService/./VideoConvertExecuteService.csproj"
COPY . .
WORKDIR "/src/Services/VideoConvert/VideoConvertExecuteService"
RUN dotnet build "./VideoConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VideoConvertExecuteService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoConvertExecuteService.dll"]