# Build Stage (This part remains the same)
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

# Runtime Stage (This part changes)
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

# Copy full source and published output from the build stage
COPY --from=build /src ./
COPY --from=build /out ./out

# Install EF CLI
RUN dotnet tool install --global dotnet-ef

# Set environment variable for the EF tool path
ENV PATH="${PATH}:/root/.dotnet/tools"

# =================================================================
# NEW INSTRUCTIONS ▼
# =================================================================
# Copy the new entrypoint script into the image
COPY entrypoint.sh .
# Make the script executable
RUN chmod +x ./entrypoint.sh

# Set the new entrypoint script as the command to run when the container starts
ENTRYPOINT ["./entrypoint.sh"]