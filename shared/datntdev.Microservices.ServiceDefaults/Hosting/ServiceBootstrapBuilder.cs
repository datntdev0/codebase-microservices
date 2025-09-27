using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public static class ServiceBootstrapBuilder
    {
        public static IHost CreateHostApplication<TStartup>(string[] args) where TStartup : AppServiceStartup
        {
            var builder = Host.CreateApplicationBuilder(args);
            var startup = Activator.CreateInstance(typeof(TStartup), builder.Environment);
            ((TStartup)startup!).ConfigureServices(builder.Services);
            var app = builder.Build();
            return app;
        }

        public static IHost CreateWebApplication<TStartup>(string[] args) where TStartup : WebServiceStartup
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = Activator.CreateInstance(typeof(TStartup), builder.Environment);
            ((TStartup)startup!).ConfigureServices(builder.Services);
            var app = builder.Build();
            ((TStartup)startup).Configure(app, builder.Environment);
            return app;
        }
    }
}
