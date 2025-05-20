# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Faces/Faces.API/Faces.API.csproj", "Faces/Faces.API/"]
COPY ["Faces/Faces.Core/Faces.Core.csproj", "Faces/Faces.Core/"]

RUN dotnet restore "Faces/Faces.API/Faces.API.csproj"

COPY ["Faces/Faces.API", "Faces/Faces.API/"]
COPY ["Faces/Faces.Core", "Faces/Faces.Core/"]

WORKDIR "/src/Faces/Faces.API"

RUN dotnet build "Faces.API.csproj" -c $BUILD_CONFIGURATION --no-restore --no-incremental && \
    dotnet publish "Faces.API.csproj" -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Faces.API.dll"]