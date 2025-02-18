using System.Threading.Tasks;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;
using datntdev.Abp.Runtime.Caching;
using datntdev.Abp.Runtime.Session;

namespace datntdev.Abp.Domain.Entities.Caching
{
    public abstract class MultiTenancyEntityCache<TEntity, TCacheItem, TPrimaryKey> :
        EntityCacheBase<TEntity, TCacheItem, TPrimaryKey>,
        IEventHandler<EntityChangedEventData<TEntity>>,
        IMultiTenancyEntityCache<TCacheItem, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public ITypedCache<string, TCacheItem> InternalCache => CacheManager.GetCache<string, TCacheItem>(CacheName);

        public IAbpSession AbpSession { get; set; }

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MultiTenancyEntityCache(
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TEntity, TPrimaryKey> repository,
            string cacheName = null)
            : base(
                cacheManager,
                repository,
                unitOfWorkManager,
                cacheName)
        {
            _unitOfWorkManager = unitOfWorkManager;

            AbpSession = NullAbpSession.Instance;
        }

        public override TCacheItem Get(TPrimaryKey id)
        {
            return InternalCache.Get(GetCacheKey(id), () => GetCacheItemFromDataSource(id));
        }

        public override Task<TCacheItem> GetAsync(TPrimaryKey id)
        {
            return InternalCache.GetAsync(GetCacheKey(id), async () => await GetCacheItemFromDataSourceAsync(id));
        }

        public virtual void HandleEvent(EntityChangedEventData<TEntity> eventData)
        {
            InternalCache.Remove(GetCacheKey(eventData.Entity));
        }

        protected virtual int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }

        public virtual string GetCacheKey(TPrimaryKey id)
        {
            return GetCacheKey(id, GetCurrentTenantId());
        }

        public virtual string GetCacheKey(TPrimaryKey id, int? tenantId)
        {
            return id + "@" + (tenantId ?? 0);
        }

        protected abstract string GetCacheKey(TEntity entity);

        public override string ToString()
        {
            return $"MultiTenancyEntityCache {CacheName}";
        }
    }
}
