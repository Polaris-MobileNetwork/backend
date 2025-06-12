#!/bin/bash

# Exit immediately if a command exits with a non-zero status.
set -e

# Define connection string and other parameters
# Note: These values will be passed in from docker-compose environment variables
CONNECTION_STRING="Server=sqlserver,1433;Database=PolarisDb;User Id=sa;Password=${DB_SA_PASSWORD};TrustServerCertificate=True;"
MAX_RETRIES=10
RETRY_INTERVAL=5

# Loop to wait for the database to be ready and apply migrations
for i in $(seq 1 $MAX_RETRIES); do
    echo "Attempt $i: Running EF Core Migrations..."
    # The -- -- before the connection string is important to separate arguments
    if dotnet ef database update --project Infrastructure --startup-project WebAPI -- --connection "$CONNECTION_STRING"; then
        echo "Migration successful!"
        break
    else
        echo "Migration failed. Retrying in $RETRY_INTERVAL seconds..."
        sleep $RETRY_INTERVAL
    fi

    if [ $i -eq $MAX_RETRIES ]; then
        echo "Max retries reached. Could not apply migrations."
        exit 1
    fi
done

# After migrations are successful, start the main application
echo "Starting WebAPI..."
# Change directory to the published output
cd /app/out
# Execute the main application DLL
dotnet WebAPI.dll