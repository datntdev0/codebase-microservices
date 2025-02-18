using datntdev.Abp.Auditing;
using datntdev.Abp.RealTime;

namespace datntdev.Abp.Web.Core.SignalR.Hubs;

public class AbpCommonHub : OnlineClientHubBase
{
    public AbpCommonHub(IOnlineClientManager onlineClientManager, IOnlineClientInfoProvider clientInfoProvider)
        : base(onlineClientManager, clientInfoProvider)
    {
    }

    public void Register()
    {
        Logger.Debug("A client is registered: " + Context.ConnectionId);
    }
}
