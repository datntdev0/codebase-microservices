using datntdev.Microservices.ServiceDefaults.Hosting;
using Scalar.AspNetCore;

namespace datntdev.Microservices.Srv.Admin.Web.Host;

internal class Startup(IWebHostEnvironment env) : WebServiceStartup<SrvAdminWebHostModule>(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddDefaultHealthChecks();
        services.AddDefaultOpenTelemetry();
        services.AddDefaultServiceDiscovery();
        services.AddAppServiceAsControllers();
        services.AddOpenApi();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.Configure(app, env);

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(configure =>
        {
            configure.MapControllers();
            
            if (env.IsDevelopment()) 
            { 
                configure.MapRootToScalar();
                configure.MapDefaultHealthChecks();
            }
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
