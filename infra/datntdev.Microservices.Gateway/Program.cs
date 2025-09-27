using datntdev.Microservices.ServiceDefaults.Hosting;

namespace datntdev.Microservices.Gateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDefaultHealthChecks();
        builder.Services.AddDefaultOpenTelemetry();
        builder.Services.AddDefaultServiceDiscovery();

        var yarpConfig = builder.Configuration.GetSection("ReverseProxy");
        builder.Services.AddReverseProxy().LoadFromConfig(yarpConfig)
            .AddServiceDiscoveryDestinationResolver();

        var app = builder.Build();
        app.MapReverseProxy();

        app.MapDefaultHealthChecks();

        app.Run();
    }
}
