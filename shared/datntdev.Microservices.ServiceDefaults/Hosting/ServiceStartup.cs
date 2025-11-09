using datntdev.Microservices.Common.Configuration;
using datntdev.Microservices.Common.Modular;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

    public abstract class AppServiceStartup<TBootstrapModule>(IHostEnvironment env) : IAppServiceStartup
        where TBootstrapModule : BaseModule
    {
        protected readonly IHostEnvironment _hostingEnvironment = env;
        protected readonly IConfigurationRoot _hostingConfiguration = AppConfiguration.Get(env);

        public virtual void ConfigureServices(IServiceCollection services) 
        {
            services.AddServiceBootstrap<TBootstrapModule>(_hostingConfiguration);
            services.AddCorsOriginsFromConfiguration(_hostingConfiguration);
        }
    }

    public abstract class WebServiceStartup<TBootstrapModule>(IWebHostEnvironment env) 
        : AppServiceStartup<TBootstrapModule>(env), IWebServiceStartup
        where TBootstrapModule : BaseModule
    {
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            app.UseServiceBootstrap<TBootstrapModule>(_hostingConfiguration);
        }
    }
}
