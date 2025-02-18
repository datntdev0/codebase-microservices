using datntdev.Abp.Modules;

namespace datntdev.Abp.Castle.Logging.Log4Net
{
    /// <summary>
    /// ABP Castle Log4Net module.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpCastleLog4NetModule : AbpModule
    {

    }
}