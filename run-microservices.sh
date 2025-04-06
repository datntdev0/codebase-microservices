#!/bin/bash

# Check development certificates
if [ ! -f "./etc/dev-cert/localhost.pfx" ]; then
    echo "Creating dev certificates..."
    cd "./etc/dev-cert"
    ./create-certificate.sh
    cd ../..
fi

# Check Docker containers
docker network create eshoponabp-network

required_services=("postgres-db" "rabbitmq" "redis")

for service in "${required_services[@]}"; do
    if docker ps --filter "name=$service" | grep -q "$service"; then
        echo "$service [up]"
    else
        cd "./etc/docker/"
        docker-compose -f docker-compose.infrastructure.yml -f docker-compose.infrastructure.override.yml up -d
        cd ../..
        break
    fi
done

# Run all services
dotnet run --project aspire/AppHost/eShopOnAbp.AppHost.csproj 