# codebase-microservices

Component | Code Quality | Security Rate | Code Coverage | Code Smells
---|---|---|---|---
Frontend: | ![SonarCloud Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-frontend&metric=alert_status) | [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-frontend&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-frontend) | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-backend&metric=coverage)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-backend) | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-frontend&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-frontend)
Backend: | ![SonarCloud Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-backend&metric=alert_status) |[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-backend&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-backend) | [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-backend&metric=coverage)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-backend) | [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=datntdev0_codebase-microservices-backend&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=datntdev0_codebase-microservices-backend)

## Overview

A modern microservice boilerplate for rapid SaaS development with .NET and Angular. Features built-in multi-tenancy, secure authentication (OpenIddict, SSO), event-driven architecture (Kafka, gRPC), real-time notifications, and cloud-ready CI/CD and monitoring. Start scalable distributed projects fast with best practices out of the box.

![](./docs/9.attachments/01.software-architecture-light.svg)

- Modular microservice architecture with DDD, EDD, TDD principles
- Built on .NET Aspire for cloud-native microservices
- Angular SPA frontend with theming and localization
- Multi-tenancy support with customizable tenant branding
- OAuth2/OIDC authentication with OpenIddict
- Event-driven communication using Kafka and gRPC
- Realtime in-app notifications with SignalR
- Payment integration for subscription management
- CI/CD pipelines with automated testing and quality scans
- Monitoring and alerting with Grafana Cloud

## Getting Started

### Prerequisites

- Docker Desktop or Docker Engine
- Visual Studio 2022 with .NET workloads
- Visual Studio Code and NodeJs version 22 
- Database GUI tools (SSMS, MongoDB Compass, RedisInsight)

Execute the following commands in the terminal:

### Run the Application

```bash
# 1. Start Docker Desktop to run the required infrastructure services.
docker compose -f .github/dockers/docker-compose.yml -p datntdev_microservices_infra up -d
# 2. Run the database migrations and start the application.
dotnet run --project ./infra/datntdev.Microservices.Migrator/datntdev.Microservices.Migrator.csproj
# 3. Start the microservices with Aspire Orchestrator.
dotnet run --project ./apps/datntdev.Microservices.AppHost/datntdev.Microservices.AppHost.csproj
# 4. Start the frontend application on new termninal.
cd ./apps/datntdev.Microservices.Angular && yarn && yarn start
```

To stop and cleanup the infrastructure services, run:
```bash
docker compose -f .github/dockers/docker-compose.yml -p datntdev_microservices_infra down -v
```
