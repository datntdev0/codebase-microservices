using datntdev.Abp.Application.Features;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.MultiTenancy;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Editions;

namespace datntdev.Microservice.MultiTenancy;

public class TenantManager : AbpTenantManager<Tenant, User>
{
    public TenantManager(
        IRepository<Tenant> tenantRepository,
        IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
        EditionManager editionManager,
        IAbpZeroFeatureValueStore featureValueStore)
        : base(
            tenantRepository,
            tenantFeatureRepository,
            editionManager,
            featureValueStore)
    {
    }
}
