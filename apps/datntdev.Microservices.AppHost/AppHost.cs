var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.datntdev_Microservices_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.datntdev_Microservices_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.AddProject<Projects.datntdev_Microservices_Srv_Identity_Web_Host>("srv-identity")
    .WithHttpHealthCheck("/health");
builder.AddProject<Projects.datntdev_Microservices_Srv_Admin_Web_Host>("srv-admin")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
