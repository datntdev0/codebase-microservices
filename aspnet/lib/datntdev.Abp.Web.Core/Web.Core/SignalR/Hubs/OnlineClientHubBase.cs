﻿using System;
using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.RealTime;
using datntdev.Abp.Runtime.Session;
using Castle.Core.Logging;

namespace datntdev.Abp.Web.Core.SignalR.Hubs;

public abstract class OnlineClientHubBase : AbpHubBase, ITransientDependency
{
    protected IOnlineClientManager OnlineClientManager { get; }
    protected IOnlineClientInfoProvider OnlineClientInfoProvider { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AbpCommonHub"/> class.
    /// </summary>
    protected OnlineClientHubBase(
        IOnlineClientManager onlineClientManager,
        IOnlineClientInfoProvider clientInfoProvider)
    {
        OnlineClientManager = onlineClientManager;
        OnlineClientInfoProvider = clientInfoProvider;

        Logger = NullLogger.Instance;
#pragma warning disable CS0618 // Type or member is obsolete, this line will be removed once the AbpSession property is removed
        AbpSession = NullAbpSession.Instance;
#pragma warning restore CS0618 // Type or member is obsolete, this line will be removed once the AbpSession property is removed
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var client = CreateClientForCurrentConnection();

        Logger.Debug("A client is connected: " + client);

        await OnlineClientManager.AddAsync(client);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);

        Logger.Debug("A client is disconnected: " + Context.ConnectionId);

        try
        {
            await OnlineClientManager.RemoveAsync(Context.ConnectionId);
        }
        catch (Exception ex)
        {
            Logger.Warn(ex.ToString(), ex);
        }
    }

    protected virtual IOnlineClient CreateClientForCurrentConnection()
    {
        return OnlineClientInfoProvider.CreateClientForCurrentConnection(Context);
    }
}
