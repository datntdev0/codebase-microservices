var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.datntdev_Microservices_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.datntdev_Microservices_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
