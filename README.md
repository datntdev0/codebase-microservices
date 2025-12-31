# codebase-microservices

**Project Vision Statement:** To empower development teams to launch enterprise-grade, multi-tenant SaaS products in days rather than months by providing a standardized, reusable microservice boilerplate that enables teams to initialize production-ready SaaS projects, with built-in multi-tenancy, authorization, real-time notifications, and other common enterprise-grade features.

> **💡 Why Choose This Boilerplate?** The goal of this project is to bridge the gap between a 'Hello World' microservice and a production-ready SaaS. We've handled the heavy lifting—Identity, Authorization, Localization, and CI/CD—so you can focus 100% on your business logic.

## 🎯 Key Features

- ⚡ **Enterprise-Grade Multi-Tenancy:**  
    Launch SaaS products with confidence. Our architecture supports logical data isolation and tenant-specific configurations out of the box, ensuring that your customers' data remains secure and private while sharing a scalable infrastructure.
- ⚡ **Accelerated "Day Zero" Development:**  
    Stop reinventing the wheel. This boilerplate integrates .NET Aspire for a seamless local orchestration experience, allowing you to spin up a complex ecosystem of SQL Server, MongoDB, Kafka, and Redis with a single command.
- ⚡ **Event-Driven Consistency with Kafka:**  
    Built for scale using Apache Kafka as the distributed backbone. We implement the Outbox pattern to ensure reliable cross-service communication and "source of truth" state changes, preventing data loss during network partitions.
- ⚡ **Deep Observability & Trust:**  
    - *100% Traceability:* Integrated OpenTelemetry for distributed tracing across YARP Gateway and microservices.  
    - *Real-time Health:* Pre-configured Prometheus & Grafana dashboards.  
    - *Security First:* OWASP-aligned security headers, encrypted data at rest, and automated secret management.


### 💯 Architectural Excellence (DDD & SOLID)
- **Domain-Driven Design:** Clear Bounded Contexts to prevent "Big Ball of Mud" architectures.
- **Hybrid Communication:** High-performance gRPC for internal service-to-service calls and REST via YARP for edge routing.
- **Testing-Ready:** A robust TDD environment featuring pre-configured Unit, Integration, and E2E tests using .NET Aspire.

### 🛠️ Tech Stack & Performance Target

Feature | Technology | Performance Target
--------|------------|-------------------
Database | SQL Server & MongoDB | ACID + Flexible Schemas with EF Core Optimized
Backend | .NET Aspire with ASP.NET Core | <300ms Response Time, 1000 RPS per Instance
Frontend | Angular SPA with Bootstrap | Lighthouse 90+ Performance Scores
API Gateway | Yet Another Reverse Proxy (YARP) | Lightning-fast Routing, 10,000 RPS per Instance
Messaging | Apache Kafka | Event-Driven, Scalable, Durable Messaging
Observability | Prometheus & Grafana | 100% Traceability, Real-Time Monitoring & Alerts
Deployment | GitHub Actions & Docker | Automated CI/CD with Containerization

## 🚀 Getting Started

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
