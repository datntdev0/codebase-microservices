using eShopOnAbp.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var profile = "kestrel";

// Microservices
var adminService = builder.AddProject<Projects.EShopOnAbp_AdminService_WebApi_Host>("adminService", profile);
var identityService = builder.AddProject<Projects.EShopOnAbp_IdentityService_HttpApi_Host>("identityService", profile);
var cmsKitService = builder.AddProject<Projects.EShopOnAbp_CmskitService_HttpApi_Host>("cmsKitService", profile);
var orderingService = builder.AddProject<Projects.EShopOnAbp_OrderingService_HttpApi_Host>("orderingService", profile);
var paymentService = builder.AddProject<Projects.EShopOnAbp_PaymentService_HttpApi_Host>("paymentService", profile);

var catalogService = builder.AddProject<Projects.EShopOnAbp_CatalogService_WebApi_Host>("catalogService", profile)
    .WithEndpoint(
        endpointName: "grpc",
        callback: static endpoint =>
        {
            endpoint.Port = 8181;
            endpoint.UriScheme = "http";
            endpoint.Transport = "http2";
            endpoint.IsProxied = false;
        }
    );

var basketService = builder.AddProject<Projects.EShopOnAbp_BasketService_HttpApi_Host>("basketService", profile)
    .WithReference(catalogService);

// Gateways
var webGateway = builder.AddProject<Projects.EShopOnAbp_WebGateway>("webGateway")
    .WithReference(adminService)
    .WithReference(identityService)
    .WithReference(catalogService)
    .WithReference(basketService)
    .WithReference(cmsKitService)
    .WithReference(orderingService)
    .WithReference(paymentService);
var webPublicGateway = builder.AddProject<Projects.EShopOnAbp_WebPublicGateway>("webPublicGateway")
    .WithReference(adminService)
    .WithReference(identityService)
    .WithReference(catalogService)
    .WithReference(basketService)
    .WithReference(cmsKitService)
    .WithReference(orderingService)
    .WithReference(paymentService);

// Apps
var publicWebApp = builder.AddProject<Projects.EShopOnAbp_PublicWeb>("public-web", "https")
    .WithExternalHttpEndpoints()
    .WithReference(catalogService)
    .WithReference(webPublicGateway);

builder.Build().Run();