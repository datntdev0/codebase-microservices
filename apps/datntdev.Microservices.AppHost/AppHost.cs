using SrvAdmin = Projects.datntdev_Microservices_Srv_Admin_Web_Host;
using SrvIdentity = Projects.datntdev_Microservices_Srv_Identity_Web_Host;
using Gateway = Projects.datntdev_Microservices_Gateway;

var builder = DistributedApplication.CreateBuilder(args);

var srvAdmin = builder.AddProject<SrvAdmin>("srv-admin")
    .WithHttpHealthCheck("/health");
var srvIdentity = builder.AddProject<SrvIdentity>("srv-identity")
    .WithHttpHealthCheck("/health");

builder.AddProject<Gateway>("gateway")
    .WithReference(srvAdmin).WaitFor(srvAdmin).WithExternalHttpEndpoints()
    .WithReference(srvIdentity).WaitFor(srvIdentity).WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
