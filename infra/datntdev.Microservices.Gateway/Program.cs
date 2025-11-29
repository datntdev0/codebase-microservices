using datntdev.Microservices.ServiceDefaults.Hosting;

namespace datntdev.Microservices.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure the services for Yarp Gateway

        builder.Services.AddDefaultHealthChecks();
        builder.Services.AddDefaultOpenTelemetry();
        builder.Services.AddDefaultServiceDiscovery();
        builder.Services.AddCorsOriginsFromConfiguration(builder.Configuration);

        var yarpConfig = builder.Configuration.GetSection("ReverseProxy");
        builder.Services.AddReverseProxy().LoadFromConfig(yarpConfig)
            .AddServiceDiscoveryDestinationResolver();

        // Build the app and use configured services

        var app = builder.Build();

        app.UseCors();

        app.MapReverseProxy();

        app.MapDefaultHealthChecks();

        app.Run();
    }
}
