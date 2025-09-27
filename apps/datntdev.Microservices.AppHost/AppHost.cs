var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservices_Srv_Identity_Web_Host>("srv-identity")
    .WithHttpHealthCheck("/health");
builder.AddProject<Projects.datntdev_Microservices_Srv_Admin_Web_Host>("srv-admin")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
