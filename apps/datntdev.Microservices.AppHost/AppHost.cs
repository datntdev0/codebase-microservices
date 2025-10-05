using SrvAdmin = Projects.datntdev_Microservices_Srv_Admin_Web_Host;
using SrvNotify = Projects.datntdev_Microservices_Srv_Notify_Web_Host;
using SrvPayment = Projects.datntdev_Microservices_Srv_Payment_Web_Host;
using SrvIdentity = Projects.datntdev_Microservices_Srv_Identity_Web_Host;
using Gateway = Projects.datntdev_Microservices_Gateway;

var builder = DistributedApplication.CreateBuilder(args);

var paramPassword = builder.AddParameter("param-Password", "Password!123");

// Although using ContainerLifetime.Persistent keeps container running after Aspire shuts down,
// It is still not able to connect to the database if Aspire is not running.
// Accordingly, we should always run Aspire when we want to work with the databases.
// Waiting for next release of Aspire to see if this issue is resolved.
// https://github.com/dotnet/aspire/issues/7046.

var dbInstanceSqlServer = builder.AddSqlServer("dbSqlServer", password: paramPassword, port: 1433);
var dbSrvIdentity = dbInstanceSqlServer.AddDatabase("db-SrvIdentity", "db.Identity");
var dbSrvPayment = dbInstanceSqlServer.AddDatabase("db-SrvPayment", "db.Payment");

var dbInstanceMongoDb = builder.AddMongoDB("dbMongoDb", password: paramPassword, port: 27017);
var dbSrvNotify = dbInstanceMongoDb.AddDatabase("db-SrvNotify", "db-Notify");
var dbSrvAdmin = dbInstanceMongoDb.AddDatabase("db-SrvAdmin", "db-Admin");

var srvAdmin = builder.AddProject<SrvAdmin>("srv-admin")
    .WithReference(dbSrvAdmin, "Default").WaitFor(dbSrvAdmin)
    .WithHttpHealthCheck("/alive");
var srvNotify = builder.AddProject<SrvNotify>("srv-notify")
    .WithReference(dbSrvNotify, "Default").WaitFor(dbSrvNotify)
    .WithHttpHealthCheck("/alive");
var srvPayment = builder.AddProject<SrvPayment>("srv-payment")
    .WithReference(dbSrvPayment, "Default").WaitFor(dbSrvPayment)
    .WithHttpHealthCheck("/alive");
var srvIdentity = builder.AddProject<SrvIdentity>("srv-identity")
    .WithReference(dbSrvIdentity, "Default").WaitFor(dbSrvIdentity)
    .WithHttpHealthCheck("/alive");

builder.AddProject<Gateway>("gateway")
    .WithReference(srvAdmin).WaitFor(srvAdmin).WithExternalHttpEndpoints()
    .WithReference(srvNotify).WaitFor(srvNotify).WithExternalHttpEndpoints()
    .WithReference(srvPayment).WaitFor(srvPayment).WithExternalHttpEndpoints()
    .WithReference(srvIdentity).WaitFor(srvIdentity).WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
