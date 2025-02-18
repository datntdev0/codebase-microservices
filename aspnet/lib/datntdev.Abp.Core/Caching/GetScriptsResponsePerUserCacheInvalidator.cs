using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Configuration;
using datntdev.Abp.Dependency;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;
using datntdev.Abp.Localization;
using datntdev.Abp.Organizations;
using datntdev.Abp.CachedUniqueKeys;

namespace datntdev.Abp.Caching
{
    public class GetScriptsResponsePerUserCacheInvalidator :
        IEventHandler<EntityChangedEventData<UserPermissionSetting>>,
        IEventHandler<EntityChangedEventData<UserRole>>,
        IEventHandler<EntityChangedEventData<UserOrganizationUnit>>,
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,
        IEventHandler<EntityChangedEventData<OrganizationUnitRole>>,
        IEventHandler<EntityChangedEventData<LanguageInfo>>,
        IEventHandler<EntityChangedEventData<SettingInfo>>,
        ITransientDependency
    {
        private const string CacheName = "GetScriptsResponsePerUser";

        private readonly ICachedUniqueKeyPerUser _cachedUniqueKeyPerUser;

        public GetScriptsResponsePerUserCacheInvalidator(ICachedUniqueKeyPerUser cachedUniqueKeyPerUser)
        {
            _cachedUniqueKeyPerUser = cachedUniqueKeyPerUser;
        }

        public void HandleEvent(EntityChangedEventData<UserPermissionSetting> eventData)
        {
            _cachedUniqueKeyPerUser.RemoveKey(CacheName, eventData.Entity.TenantId, eventData.Entity.UserId);
        }

        public void HandleEvent(EntityChangedEventData<UserRole> eventData)
        {
            _cachedUniqueKeyPerUser.RemoveKey(CacheName, eventData.Entity.TenantId, eventData.Entity.UserId);
        }

        public void HandleEvent(EntityChangedEventData<UserOrganizationUnit> eventData)
        {
            _cachedUniqueKeyPerUser.RemoveKey(CacheName, eventData.Entity.TenantId, eventData.Entity.UserId);
        }

        public void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            _cachedUniqueKeyPerUser.RemoveKey(CacheName, eventData.Entity.TenantId, eventData.Entity.Id);
        }

        public void HandleEvent(EntityChangedEventData<OrganizationUnitRole> eventData)
        {
            _cachedUniqueKeyPerUser.ClearCache(CacheName);
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
}