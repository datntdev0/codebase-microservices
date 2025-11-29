using datntdev.Microservices.Common;
using datntdev.Microservices.ServiceDefaults.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservices.Migrator;

internal class Startup(IHostEnvironment env) 
    : AppServiceStartup<MigratorModule>(env, Constants.Enum.ServiceType.Migrator)
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

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
