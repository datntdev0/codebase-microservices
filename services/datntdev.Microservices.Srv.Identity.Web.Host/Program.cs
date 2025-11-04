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

        // Configure CORS
        var corsOrigins = _hostingConfiguration.GetSection("AllowedOrigins").Get<string>()?.Split(';') ?? [];
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if (corsOrigins.Length > 0)
                {
                    policy.WithOrigins(corsOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                }
            });
        });

        services.AddRazorComponents();
        services.AddControllers();
        services.AddOpenApi();

        services.AddTransient<AppSessionMiddleware>();
    }

    public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        base.Configure(app, env);

        app.UseHttpsRedirection();
        
        // Enable CORS (must be before UseRouting)
        app.UseCors();
        
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
