using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.Microservice.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace datntdev.Microservice.Web.Host.Startup
{
    [DependsOn(
       typeof(MicroserviceWebCoreModule))]
    public class MicroserviceWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MicroserviceWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MicroserviceWebHostModule).GetAssembly());
        }
    }
}
