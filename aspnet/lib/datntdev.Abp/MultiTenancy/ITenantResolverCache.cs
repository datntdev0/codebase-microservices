using JetBrains.Annotations;

namespace datntdev.Abp.MultiTenancy
{
    public interface ITenantResolverCache
    {
        [CanBeNull]
        TenantResolverCacheItem Value { get; set; }
    }
}