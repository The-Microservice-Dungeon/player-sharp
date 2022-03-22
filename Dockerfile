#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["src", "."]
RUN dotnet restore "./Sharp.Player/Sharp.Player.csproj"
COPY . .
WORKDIR "/src/Sharp.Player"
RUN dotnet build "Sharp.Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sharp.Player.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sharp.Player.dll"]