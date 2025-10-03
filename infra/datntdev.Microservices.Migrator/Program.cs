using datntdev.Microservices.ServiceDefaults.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservices.Migrator;

internal class Startup(IHostEnvironment env) : AppServiceStartup(env)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceBootstrap<MigratorModule>(_hostingConfiguration);
        services.AddHostedService<MigratorHostedWorker>();
    }
}

public partial class Program
{
    public static void Main(string[] args)
    {
        ServiceBootstrapBuilder.CreateHostApplication<Startup>(args).Run();
    }
}
