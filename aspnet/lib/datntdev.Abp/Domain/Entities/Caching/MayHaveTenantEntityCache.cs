using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Runtime.Caching;

namespace datntdev.Abp.Domain.Entities.Caching
{
    public class MayHaveTenantEntityCache<TEntity, TCacheItem> :
        MayHaveTenantEntityCache<TEntity, TCacheItem, int>,
        IMultiTenancyEntityCache<TCacheItem>
        where TEntity : class, IEntity<int>, IMayHaveTenant
    {
        public MayHaveTenantEntityCache(
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TEntity, int> repository,
            string cacheName = null)
            : base(
                cacheManager,
                unitOfWorkManager,
                repository,
                cacheName)
        {
        }
    }

    public class MayHaveTenantEntityCache<TEntity, TCacheItem, TPrimaryKey> : MultiTenancyEntityCache<TEntity, TCacheItem, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>, IMayHaveTenant
    {
        public MayHaveTenantEntityCache(
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<TEntity, TPrimaryKey> repository,
            string cacheName = null)
            : base(
                cacheManager,
                unitOfWorkManager,
                repository,
                cacheName)
        {
        }

        protected override string GetCacheKey(TEntity entity)
        {
            return GetCacheKey(entity.Id, entity.TenantId);
        }

        public override string ToString()
        {
            return $"MayHaveTenantEntityCache {CacheName}";
        }
    }
}
