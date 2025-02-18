using System.Threading.Tasks;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;
using datntdev.Abp.Runtime.Caching;

namespace datntdev.Abp.Domain.Entities.Caching
{
    public class EntityCache<TEntity, TCacheItem> :
        EntityCache<TEntity, TCacheItem, int>,
        IEntityCache<TCacheItem>
        where TEntity : class, IEntity<int>
    {
        public EntityCache(
            ICacheManager cacheManager,
            IRepository<TEntity, int> repository,
            IUnitOfWorkManager unitOfWorkManager,
            string cacheName = null)
            : base(
                cacheManager,
                repository,
                unitOfWorkManager,
                cacheName)
        {
        }
    }

    public class EntityCache<TEntity, TCacheItem, TPrimaryKey> :
        EntityCacheBase<TEntity, TCacheItem, TPrimaryKey>,
        IEventHandler<EntityChangedEventData<TEntity>>, 
        IEntityCache<TCacheItem, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public ITypedCache<TPrimaryKey, TCacheItem> InternalCache
        {
            get
            {
                return CacheManager.GetCache<TPrimaryKey, TCacheItem>(CacheName);
            }
        }

        public EntityCache(
            ICacheManager cacheManager, 
            IRepository<TEntity, TPrimaryKey> repository,
            IUnitOfWorkManager unitOfWorkManager,
            string cacheName = null)
            : base(
                cacheManager,
                repository,
                unitOfWorkManager,
                cacheName)
        {
        }

        public override TCacheItem Get(TPrimaryKey id)
        {
            return InternalCache.Get(id, () => GetCacheItemFromDataSource(id));
        }

        public override Task<TCacheItem> GetAsync(TPrimaryKey id)
        {
            return InternalCache.GetAsync(id, () => GetCacheItemFromDataSourceAsync(id));
        }

        public virtual void HandleEvent(EntityChangedEventData<TEntity> eventData)
        {
            InternalCache.Remove(eventData.Entity.Id);
        }

        public override string ToString()
        {
            return $"EntityCache {CacheName}";
        }
    }
}
