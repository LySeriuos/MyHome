# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyHomeBlazorApp/MyHomeBlazorApp.csproj", "MyHomeBlazorApp/"]
COPY ["MyHome/MyHome.csproj", "MyHome/"]
RUN dotnet restore "./MyHomeBlazorApp/MyHomeBlazorApp.csproj"
COPY . .
WORKDIR "/src/MyHomeBlazorApp"
RUN dotnet build "./MyHomeBlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyHomeBlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHomeBlazorApp.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Install the graphics library needed for your QR codes
RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app

# 1. Create the folder while still ROOT
RUN mkdir -p /app/Files

# 2. Give the 'app' user ownership of the folder
# In .NET 8, the default user is 'app'
RUN chown -R app:app /app/Files && chmod -R 775 /app/Files

# 3. Switch to the non-root user last
USER app

EXPOSE 8080
EXPOSE 8081