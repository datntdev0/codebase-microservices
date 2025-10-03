using datntdev.Microservices.ServiceDefaults.Hosting;
using Scalar.AspNetCore;

namespace datntdev.Microservices.Srv.Payment.Web.Host;

internal class Startup(IWebHostEnvironment env) : WebServiceStartup(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceBootstrap<SrvPaymentWebHostModule>(_hostingConfiguration);
        services.AddDefaultHealthChecks();
        services.AddDefaultOpenTelemetry();
        services.AddDefaultServiceDiscovery();

        services.AddControllers();
        services.AddOpenApi();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseServiceBootstrap<SrvPaymentWebHostModule>(_hostingConfiguration);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(configure =>
        {
            configure.MapControllers();
            configure.MapOpenApi();
            configure.MapScalarApiReference();
            if (env.IsDevelopment()) configure.MapDefaultHealthChecks();
        });
    }
}

public partial class Program
{
    public static void Main(string[] args)
    {
        ServiceBootstrapBuilder.CreateWebApplication<Startup>(args).Run();
    }
}
