FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Tiktok.API/Tiktok.API.Presentation/Tiktok.API.Presentation.csproj", "Tiktok.API/Tiktok.API.Presentation/"]
COPY ["Tiktok.API/Tiktok.API.Infrastructure/Tiktok.API.Infrastructure.csproj", "Tiktok.API/Tiktok.API.Infrastructure/"]
COPY ["Tiktok.API/Tiktok.API.Application/Tiktok.API.Application.csproj", "Tiktok.API/Tiktok.API.Application/"]
COPY ["Tiktok.API/Tiktok.API.Domain/Tiktok.API.Domain.csproj", "Tiktok.API/Tiktok.API.Domain/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "BuildingBlocks/Infrastructure/"]
RUN dotnet restore "Tiktok.API/Tiktok.API.Presentation/Tiktok.API.Presentation.csproj"
COPY . .
WORKDIR "/src/Tiktok.API/Tiktok.API.Presentation"
RUN dotnet build "Tiktok.API.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tiktok.API.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tiktok.API.Presentation.dll"]
