using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Modular
{
    public abstract class BaseModule
    {
        public virtual void ConfigureServices(IServiceCollection services, IConfigurationRoot configs) { }
        public virtual void Configure(IServiceProvider serviceProvider, IConfigurationRoot configs) { }
    }
}
