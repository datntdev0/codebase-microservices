using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;

namespace datntdev.Abp.TestBase
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpTestBaseModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpTestBaseModule).GetAssembly());
        }
    }
}