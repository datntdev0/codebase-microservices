using datntdev.Microservices.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    public abstract class AppServiceStartup(IHostEnvironment env)
    {
        protected readonly IConfigurationRoot _hostingConfiguration = AppConfiguration.Get(env);

        public virtual void ConfigureServices(IServiceCollection services) { }
    }

    public abstract class WebServiceStartup(IWebHostEnvironment env) : AppServiceStartup(env)
    {
        protected readonly IWebHostEnvironment _hostingEnvironment = env;

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) { }
    }
}
