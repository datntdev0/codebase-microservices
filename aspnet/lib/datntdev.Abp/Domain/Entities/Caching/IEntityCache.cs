using datntdev.Abp.Runtime.Caching;

namespace datntdev.Abp.Domain.Entities.Caching
{
    public interface IEntityCache<TCacheItem> : IEntityCache<TCacheItem, int>
    {
    }

    public interface IEntityCache<TCacheItem, TPrimaryKey> : IEntityCacheBase<TCacheItem, TPrimaryKey>
    {
        ITypedCache<TPrimaryKey, TCacheItem> InternalCache { get; }
    }
}