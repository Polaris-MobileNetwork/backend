# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY Domain/*.csproj ./Domain/
COPY Application/*.csproj ./Application/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY WebAPI/*.csproj ./WebAPI/

RUN dotnet restore

COPY . .

RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /out

# Runtime Stage with EF support
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copy full source and published output
COPY --from=build /src ./
COPY --from=build /out ./out

# Install EF CLI
RUN dotnet tool install --global dotnet-ef --version 8.0.5
ENV PATH="${PATH}:/root/.dotnet/tools"

# Run migrations from source, then run app from /out
ENTRYPOINT ["/bin/bash", "-c", "\
  echo 'Waiting for SQL Server...'; \
  sleep 30; \
  echo 'Running EF Core Migrations...'; \
  dotnet ef database update --project Infrastructure --startup-project WebAPI --connection 'Server=sqlserver,1433;Database=PolarisDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;' && \
  echo 'Migration complete. Starting WebAPI...' && \
  cd /app/out && \
  dotnet WebAPI.dll"]
