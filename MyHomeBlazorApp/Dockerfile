# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 1. Base Stage: Setup the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install SkiaSharp dependencies (NOT libgdiplus)
# libfontconfig1 and libfreetype6 are the "muscles" for SkiaSharp on Linux
USER root
RUN apt-get update && apt-get install -y \
    libfontconfig1 \
    libfreetype6 \
    && rm -rf /var/lib/apt/lists/*

# Create storage and set permissions for the .NET 'app' user
RUN mkdir -p /app/Files && chown -R app:app /app/Files && chmod -R 775 /app/Files

# Switch to non-root user for security
USER app

# 2. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyHomeBlazorApp/MyHomeBlazorApp.csproj", "MyHomeBlazorApp/"]
COPY ["MyHome/MyHome.csproj", "MyHome/"]
RUN dotnet restore "./MyHomeBlazorApp/MyHomeBlazorApp.csproj"
COPY . .
WORKDIR "/src/MyHomeBlazorApp"
RUN dotnet build "./MyHomeBlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publish Stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyHomeBlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# 4. Final Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Switch to root to ensure the DB folder exists and is writable
USER root
RUN mkdir -p /app/BlazorData /app/Files && \
    chown -R app:app /app/BlazorData /app/Files && \
    chmod -R 775 /app/BlazorData /app/Files

USER app
ENTRYPOINT ["dotnet", "MyHomeBlazorApp.dll"]