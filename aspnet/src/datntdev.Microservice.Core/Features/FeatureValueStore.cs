using datntdev.Abp.Application.Features;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Runtime.Caching;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.MultiTenancy;

namespace datntdev.Microservice.Features;

public class FeatureValueStore : AbpFeatureValueStore<Tenant, User>
{
    public FeatureValueStore(
        ICacheManager cacheManager,
        IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
        IRepository<Tenant> tenantRepository,
        IRepository<EditionFeatureSetting, long> editionFeatureRepository,
        IFeatureManager featureManager,
        IUnitOfWorkManager unitOfWorkManager)
        : base(
              cacheManager,
              tenantFeatureRepository,
              tenantRepository,
              editionFeatureRepository,
              featureManager,
              unitOfWorkManager)
    {
    }
}
