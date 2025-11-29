using datntdev.Microservices.Common.Configuration;
using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.ServiceDefaults.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ServiceType = datntdev.Microservices.Common.Constants.Enum.ServiceType;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public interface IAppServiceStartup
    {
        public virtual void ConfigureServices(IServiceCollection services) { }
    }

    public interface IWebServiceStartup : IAppServiceStartup
    {
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
    }

    public abstract class AppServiceStartup
    {
        public static ServiceType ServiceType { get; private set; }
        public static IHostEnvironment HostingEnvironment { get; private set; } = default!;
        public static IConfigurationRoot HostingConfiguration { get; private set; } = default!;

        public AppServiceStartup(IHostEnvironment env, ServiceType type = ServiceType.Microservice)
        {
            ServiceType = type;
            HostingEnvironment = env;
            HostingConfiguration = AppConfiguration.Get(env);
        }
    }

    public abstract class AppServiceStartup<TBootstrapModule>(
        IHostEnvironment env, ServiceType type = ServiceType.Microservice
    ) : AppServiceStartup(env, type), IAppServiceStartup
        where TBootstrapModule : BaseModule
    {
        public virtual void ConfigureServices(IServiceCollection services) 
        {
            services.AddServiceBootstrap<TBootstrapModule>(HostingConfiguration);
        }
    }

    public abstract class WebServiceStartup<TBootstrapModule>(IWebHostEnvironment env) 
        : AppServiceStartup<TBootstrapModule>(env, ServiceType.Microservice), IWebServiceStartup
        where TBootstrapModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services) 
        {
            base.ConfigureServices(services);
            services.AddCorsOriginsFromConfiguration(HostingConfiguration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            app.UseServiceBootstrap<TBootstrapModule>(HostingConfiguration);
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
