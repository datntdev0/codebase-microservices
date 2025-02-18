using datntdev.Abp.Dependency;
using Microsoft.AspNetCore.SignalR;

namespace datntdev.Abp.RealTime;

public interface IOnlineClientInfoProvider : ITransientDependency
{
    IOnlineClient CreateClientForCurrentConnection(HubCallerContext context);
}