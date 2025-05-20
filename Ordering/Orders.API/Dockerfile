# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Ordering/Orders.API/Orders.API.csproj", "Ordering/Orders.API/"]
COPY ["Ordering/Orders.Core/Orders.Core.csproj", "Ordering/Orders.Core/"]
COPY ["Messaging/Messaging.Core/Messaging.Core.csproj", "Messaging/Messaging.Core/"]
COPY ["Shared/Faces.Shared.Common/Faces.Shared.Common.csproj", "Shared/Faces.Shared.Common/"]

RUN dotnet restore "Ordering/Orders.API/Orders.API.csproj"

COPY ["Ordering/Orders.API", "Ordering/Orders.API/"]
COPY ["Ordering/Orders.Core", "Ordering/Orders.Core/"]
COPY ["Messaging/Messaging.Core", "Messaging/Messaging.Core/"]
COPY ["Shared/Faces.Shared.Common", "Shared/Faces.Shared.Common/"]

WORKDIR "/src/Ordering/Orders.API"

RUN dotnet build "Orders.API.csproj" -c $BUILD_CONFIGURATION --no-restore && \
    dotnet publish "Orders.API.csproj" -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Orders.API.dll"]