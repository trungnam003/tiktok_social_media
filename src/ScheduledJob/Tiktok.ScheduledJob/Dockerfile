FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ScheduledJob/Tiktok.ScheduledJob/Tiktok.ScheduledJob.csproj", "ScheduledJob/Tiktok.ScheduledJob/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
RUN dotnet restore "ScheduledJob/Tiktok.ScheduledJob/Tiktok.ScheduledJob.csproj"
COPY . .
WORKDIR "/src/ScheduledJob/Tiktok.ScheduledJob"
RUN dotnet build "Tiktok.ScheduledJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tiktok.ScheduledJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tiktok.ScheduledJob.dll"]

RUN apt-get -y update && apt-get -y upgrade && apt-get install -y --no-install-recommends ffmpeg