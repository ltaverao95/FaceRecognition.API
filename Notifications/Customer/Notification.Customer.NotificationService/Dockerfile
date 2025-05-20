# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Notifications/Customer/Notification.Customer.NotificationService/Notification.Customer.NotificationService.csproj", "Notifications/Customer/Notification.Customer.NotificationService/"]
COPY ["Notifications/Customer/Notification.Customer.NotificationService.Core/Notification.Customer.NotificationService.Core.csproj", "Notifications/Customer/Notification.Customer.NotificationService.Core/"]
COPY ["Notifications/Customer/Notifications.Customer.Email/Notification.Customer.EmailService.csproj", "Notifications/Customer/Notifications.Customer.Email/"]
COPY ["Messaging/Messaging.Core/Messaging.Core.csproj", "Messaging/Messaging.Core/"]

RUN dotnet restore "Notifications/Customer/Notification.Customer.NotificationService/Notification.Customer.NotificationService.csproj"

COPY ["Notifications/Customer/Notification.Customer.NotificationService", "Notifications/Customer/Notification.Customer.NotificationService/"]
COPY ["Notifications/Customer/Notification.Customer.NotificationService.Core", "Notifications/Customer/Notification.Customer.NotificationService.Core/"]
COPY ["Notifications/Customer/Notifications.Customer.Email", "Notifications/Customer/Notifications.Customer.Email/"]
COPY ["Messaging/Messaging.Core", "Messaging/Messaging.Core/"]

WORKDIR "/src/Notifications/Customer/Notification.Customer.NotificationService"

RUN dotnet build "Notification.Customer.NotificationService.csproj" -c $BUILD_CONFIGURATION --no-restore && \
    dotnet publish "Notification.Customer.NotificationService.csproj" -c $BUILD_CONFIGURATION --no-build -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Notification.Customer.NotificationService.dll"]