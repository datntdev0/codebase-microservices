FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "EShopOnAbp.sln" "EShopOnAbp.sln"

# Applications
COPY "apps/public-web/src/EShopOnAbp.PublicWeb/EShopOnAbp.PublicWeb.csproj" "apps/public-web/src/EShopOnAbp.PublicWeb/EShopOnAbp.PublicWeb.csproj"

# Shared
COPY "shared/EShopOnAbp.Shared.Hosting/EShopOnAbp.Shared.Hosting.csproj" "shared/EShopOnAbp.Shared.Hosting/EShopOnAbp.Shared.Hosting.csproj"
COPY "shared/EShopOnAbp.Shared.Hosting.AspNetCore/EShopOnAbp.Shared.Hosting.AspNetCore.csproj" "shared/EShopOnAbp.Shared.Hosting.AspNetCore/EShopOnAbp.Shared.Hosting.AspNetCore.csproj"
COPY "shared/EShopOnAbp.Shared.Hosting.Gateways/EShopOnAbp.Shared.Hosting.Gateways.csproj" "shared/EShopOnAbp.Shared.Hosting.Gateways/EShopOnAbp.Shared.Hosting.Gateways.csproj"
COPY "shared/EShopOnAbp.Shared.Hosting.Microservices/EShopOnAbp.Shared.Hosting.Microservices.csproj" "shared/EShopOnAbp.Shared.Hosting.Microservices/EShopOnAbp.Shared.Hosting.Microservices.csproj"
COPY "shared/EShopOnAbp.Shared.Localization/EShopOnAbp.Shared.Localization.csproj" "shared/EShopOnAbp.Shared.Localization/EShopOnAbp.Shared.Localization.csproj"

# Microservices
COPY "services/admin/EShopOnAbp.AdminService.WebApi.Host/EShopOnAbp.AdminService.WebApi.Host.csproj" "services/admin/EShopOnAbp.AdminService.WebApi.Host/EShopOnAbp.AdminService.WebApi.Host.csproj"
COPY "services/identity/EShopOnAbp.IdentityService.WebApi.Host/EShopOnAbp.IdentityService.WebApi.Host.csproj" "services/identity/EShopOnAbp.IdentityService.WebApi.Host/EShopOnAbp.IdentityService.WebApi.Host.csproj"
COPY "services/basket/EShopOnAbp.BasketService/EShopOnAbp.BasketService.csproj" "services/basket/EShopOnAbp.BasketService/EShopOnAbp.BasketService.csproj"
COPY "services/ordering/EShopOnAbp.OrderingService.WebApi.Host/EShopOnAbp.OrderingService.WebApi.Host.csproj" "services/ordering/EShopOnAbp.OrderingService.WebApi.Host/EShopOnAbp.OrderingService.WebApi.Host.csproj"
COPY "services/catalog/EShopOnAbp.CatalogService.WebApi.Host/EShopOnAbp.CatalogService.WebApi.Host.csproj" "services/catalog/EShopOnAbp.CatalogService.WebApi.Host/EShopOnAbp.CatalogService.WebApi.Host.csproj"
COPY "services/payment/EShopOnAbp.PaymentService.WebApi.Host/EShopOnAbp.PaymentService.WebApi.Host.csproj" "services/payment/EShopOnAbp.PaymentService.WebApi.Host/EShopOnAbp.PaymentService.WebApi.Host.csproj"
COPY "services/cmskit/EShopOnAbp.CmskitService.WebApi.Host/EShopOnAbp.CmskitService.WebApi.Host.csproj" "services/cmskit/EShopOnAbp.CmskitService.WebApi.Host/EShopOnAbp.CmskitService.WebApi.Host.csproj"

# Gateways
COPY "gateways/web/src/EShopOnAbp.WebGateway/EShopOnAbp.WebGateway.csproj" "gateways/web/src/EShopOnAbp.WebGateway/EShopOnAbp.WebGateway.csproj"
COPY "gateways/web-public/src/EShopOnAbp.WebPublicGateway/EShopOnAbp.WebPublicGateway.csproj" "gateways/web-public/src/EShopOnAbp.WebPublicGateway/EShopOnAbp.WebPublicGateway.csproj"

COPY "NuGet.Config" "NuGet.Config"

RUN dotnet restore "EShopOnAbp.sln"
COPY . .

WORKDIR "/src/services/identity/EShopOnAbp.IdentityService.WebApi.Host"
RUN dotnet publish -c Release -o /app 
# RUN dotnet publish --no-restore -c Release -o /app 

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EShopOnAbp.IdentityService.WebApi.Host.dll"]