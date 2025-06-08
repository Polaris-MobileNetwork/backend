#!/bin/bash

echo "🏁 Starting SQL Server..."
docker-compose up -d sqlserver

echo "⏳ Waiting for SQL Server to be ready..."
sleep 30

MAX_ATTEMPTS=20
ATTEMPT=1

until docker-compose exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -Q "SELECT 1" > /dev/null 2>&1; do
  if [ "$ATTEMPT" -ge "$MAX_ATTEMPTS" ]; then
    echo "❌ SQL Server did not respond after $MAX_ATTEMPTS attempts."
    exit 1
  fi
  echo "⏳ Attempt $ATTEMPT: SQL Server not ready yet..."
  ATTEMPT=$((ATTEMPT+1))
  sleep 5
done

echo "✅ SQL Server is ready!"

echo "🚀 Running EF Core migrations from source..."

docker run --rm \
  --network=polaris_polaris-network \
  -v $(pwd):/src \
  -w /src \
  mcr.microsoft.com/dotnet/sdk:8.0 \
  /bin/bash -c "
    dotnet tool install --global dotnet-ef --version 8.0.5 && \
    export PATH=\$PATH:/root/.dotnet/tools && \
    dotnet ef database update --project Infrastructure --startup-project WebAPI --connection 'Server=sqlserver,1433;Database=PolarisDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;'"

if [ $? -eq 0 ]; then
  echo "✅ Migration complete!"
  echo "👉 Now start the WebAPI: docker-compose up -d webapi"
else
  echo "❌ Migration failed."
  exit 1
fi
