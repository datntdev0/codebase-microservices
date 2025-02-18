using System.Reflection;
using datntdev.Abp.Web.Core.SignalR.Notifications;
using datntdev.Abp.Modules;

namespace datntdev.Abp.Web.Core.SignalR;

/// <summary>
/// ABP ASP.NET Core SignalR integration module.
/// </summary>
[DependsOn(typeof(AbpKernelModule))]
public class AbpWebCoreSignalRModule : AbpModule
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        Configuration.Notifications.Notifiers.Add<SignalRRealTimeNotifier>();
    }
}
