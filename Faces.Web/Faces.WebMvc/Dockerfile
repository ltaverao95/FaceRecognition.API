# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Faces.Web/Faces.WebMvc/Faces.WebMvc.csproj", "Faces.Web/Faces.WebMvc/"]
COPY ["Faces.Web/Faces.WebMvc.Core/Faces.WebMvc.Core.csproj", "Faces.Web/Faces.WebMvc.Core/"]
COPY ["Messaging/Messaging.Core/Messaging.Core.csproj", "Messaging/Messaging.Core/"]
COPY ["Shared/Faces.Shared.Common/Faces.Shared.Common.csproj", "Shared/Faces.Shared.Common/"]

RUN dotnet restore "Faces.Web/Faces.WebMvc/Faces.WebMvc.csproj"

COPY ["Faces.Web/Faces.WebMvc", "Faces.Web/Faces.WebMvc/"]
COPY ["Faces.Web/Faces.WebMvc.Core", "Faces.Web/Faces.WebMvc.Core/"]
COPY ["Messaging/Messaging.Core", "Messaging/Messaging.Core/"]
COPY ["Shared/Faces.Shared.Common", "Shared/Faces.Shared.Common/"]

WORKDIR "/src/Faces.Web/Faces.WebMvc"

RUN dotnet build "Faces.WebMvc.csproj" -c $BUILD_CONFIGURATION --no-restore && \
    dotnet publish "Faces.WebMvc.csproj" -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Faces.WebMvc.dll"]