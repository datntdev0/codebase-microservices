using datntdev.Microservices.ServiceDefaults.Hosting;
using datntdev.Microservices.ServiceDefaults.Session;
using datntdev.Microservices.Srv.Identity.Web.Host.Components;
using Scalar.AspNetCore;

namespace datntdev.Microservices.Srv.Identity.Web.Host;

internal class Startup(IWebHostEnvironment env) : WebServiceStartup<SrvIdentityWebHostModule>(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddDefaultHealthChecks();
        services.AddDefaultOpenTelemetry();
        services.AddDefaultServiceDiscovery();

        services.AddRazorComponents();
        services.AddControllers();
        services.AddOpenApi();

        services.AddTransient<AppSessionMiddleware>();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.Configure(app, env);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<AppSessionMiddleware>();

        app.UseAntiforgery();

        app.UseEndpoints(configure =>
        {
            configure.MapRazorComponents<AppIndex>();
            configure.MapStaticAssets();
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
