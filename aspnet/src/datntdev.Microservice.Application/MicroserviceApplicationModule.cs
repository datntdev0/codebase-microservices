using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Microservice.Application.Contracts;

namespace datntdev.Microservice;

[DependsOn(
    typeof(MicroserviceCoreModule),
    typeof(MicroserviceApplicationContractsModule))]
public class MicroserviceApplicationModule : AbpModule
{
    public override void Initialize()
    {
        var thisAssembly = typeof(MicroserviceApplicationModule).GetAssembly();
        IocManager.RegisterAssemblyByConvention(thisAssembly);
    }
}
