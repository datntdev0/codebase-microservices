using datntdev.Abp.AutoMapper;
using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;

namespace datntdev.Microservice.Application.Contracts
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class MicroserviceApplicationContractsModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(MicroserviceApplicationContractsModule).GetAssembly();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
