# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PolarisServer.sln ./
COPY WebAPI/WebAPI.csproj WebAPI/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

RUN dotnet restore WebAPI/WebAPI.csproj

COPY . .
WORKDIR /src/WebAPI
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
