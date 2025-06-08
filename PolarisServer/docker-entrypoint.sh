#!/bin/bash
set -e

echo "Starting Polaris Server..."

# Wait for SQL Server to be ready
echo "Waiting for SQL Server to be ready..."
until dotnet /app/WebAPI.dll --help > /dev/null 2>&1; do
  echo "Waiting for dependencies..."
  sleep 2
done

# Change to source directory for EF migrations
cd /src

# Check if database exists and run migrations
echo "Checking database connection and running migrations..."
max_attempts=30
attempt=1

while [ $attempt -le $max_attempts ]; do
  echo "Attempt $attempt of $max_attempts..."
  
  if dotnet ef database update --project Infrastructure --startup-project WebAPI --connection "$ConnectionStrings__DefaultConnection"; then
    echo "✅ Database migrations completed successfully!"
    break
  else
    echo "❌ Migration attempt $attempt failed. Retrying in 5 seconds..."
    sleep 5
    ((attempt++))
  fi
  
  if [ $attempt -gt $max_attempts ]; then
    echo "❌ Failed to apply migrations after $max_attempts attempts"
    exit 1
  fi
done

# Change back to app directory and start the application
cd /app
echo "🚀 Starting the application..."
exec dotnet WebAPI.dll