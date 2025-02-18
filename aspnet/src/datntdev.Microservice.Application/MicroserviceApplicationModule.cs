using datntdev.Abp.AutoMapper;
using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Microservice.Authorization;

namespace datntdev.Microservice;

[DependsOn(
    typeof(MicroserviceCoreModule),
    typeof(AbpAutoMapperModule))]
public class MicroserviceApplicationModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Authorization.Providers.Add<MicroserviceAuthorizationProvider>();
    }

    public override void Initialize()
    {
        var thisAssembly = typeof(MicroserviceApplicationModule).GetAssembly();

        IocManager.RegisterAssemblyByConvention(thisAssembly);

        Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(thisAssembly)
        );
    }
}
