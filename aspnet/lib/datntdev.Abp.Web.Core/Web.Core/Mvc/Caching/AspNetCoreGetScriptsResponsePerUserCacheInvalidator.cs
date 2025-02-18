using datntdev.Abp.CachedUniqueKeys;
using datntdev.Abp.Configuration;
using datntdev.Abp.Dependency;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;
using datntdev.Abp.Localization;

namespace datntdev.Abp.Web.Core.Mvc.Caching;

public class AspNetCoreGetScriptsResponsePerUserCacheInvalidator :
    IEventHandler<EntityChangedEventData<LanguageInfo>>,
    IEventHandler<EntityChangedEventData<SettingInfo>>,
    ITransientDependency
{
    private const string CacheName = "GetScriptsResponsePerUser";

    private readonly ICachedUniqueKeyPerUser _cachedUniqueKeyPerUser;

    public AspNetCoreGetScriptsResponsePerUserCacheInvalidator(ICachedUniqueKeyPerUser cachedUniqueKeyPerUser)
    {
        _cachedUniqueKeyPerUser = cachedUniqueKeyPerUser;
    }

    public void HandleEvent(EntityChangedEventData<LanguageInfo> eventData)
    {
        _cachedUniqueKeyPerUser.ClearCache(CacheName);
    }

    public void HandleEvent(EntityChangedEventData<SettingInfo> eventData)
    {
        _cachedUniqueKeyPerUser.ClearCache(CacheName);
    }
}
