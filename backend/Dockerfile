# Backend Dockerfile for .NET 8 API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files
COPY ["Zalagaonica.Backend/Zalagaonica.Backend/Zalagaonica.API.csproj", "Zalagaonica.Backend/"]
COPY ["Zalagaonica.Backend/Application/Application.csproj", "Application/"]
COPY ["Zalagaonica.Backend/Domain/Domain.csproj", "Domain/"]
COPY ["Zalagaonica.Backend/Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Zalagaonica.Backend/Zalagaonica.API.csproj"

# Copy everything else
COPY Zalagaonica.Backend/ .

# Build the application
WORKDIR "/src/Zalagaonica.Backend"
RUN dotnet build "Zalagaonica.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Zalagaonica.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:80/api/health || exit 1

ENTRYPOINT ["dotnet", "Zalagaonica.API.dll"]
