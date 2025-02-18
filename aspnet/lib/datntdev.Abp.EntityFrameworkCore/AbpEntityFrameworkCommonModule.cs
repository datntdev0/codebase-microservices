using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;

namespace datntdev.Abp.EntityFramework
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpEntityFrameworkCommonModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpEntityFrameworkCommonModule).GetAssembly());
        }
    }
}
