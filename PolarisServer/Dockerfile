# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first (for better layer caching)
COPY PolarisServer.sln ./
COPY WebAPI/WebAPI.csproj WebAPI/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Restore dependencies
RUN dotnet restore WebAPI/WebAPI.csproj

# Copy all source code
COPY . .

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef --version 8.0.5

# Add tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Build and publish the application
WORKDIR /src/WebAPI
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install dotnet-ef tool in runtime image for migrations
RUN dotnet tool install --global dotnet-ef --version 8.0.5
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy published application
COPY --from=build /app/publish .

# Copy source code needed for migrations (EF Core needs access to DbContext)
COPY --from=build /src /src
WORKDIR /src

# Create entrypoint script
COPY docker-entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.sh

WORKDIR /app

# Expose port
EXPOSE 8080

ENTRYPOINT ["/usr/local/bin/docker-entrypoint.sh"]