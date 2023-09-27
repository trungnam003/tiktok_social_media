FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tiktok.ScheduledJob/Tiktok.ScheduledJob.csproj", "Tiktok.ScheduledJob/"]
COPY ["Tiktok.API.Infrastructure/Tiktok.API.Infrastructure.csproj", "Tiktok.API.Infrastructure/"]
COPY ["Tiktok.API.Application/Tiktok.API.Application.csproj", "Tiktok.API.Application/"]
COPY ["Tiktok.API.Domain/Tiktok.API.Domain.csproj", "Tiktok.API.Domain/"]

RUN dotnet restore "Tiktok.ScheduledJob/Tiktok.ScheduledJob.csproj"
COPY . .
WORKDIR "/src/Tiktok.ScheduledJob"
RUN dotnet build "Tiktok.ScheduledJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tiktok.ScheduledJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tiktok.ScheduledJob.dll"]

RUN apt-get -y update && apt-get -y upgrade && apt-get install -y --no-install-recommends ffmpeg