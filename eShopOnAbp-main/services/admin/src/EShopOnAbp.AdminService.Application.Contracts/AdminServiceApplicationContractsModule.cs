using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

// using EShopOnAbp.CatalogService;
// using EShopOnAbp.OrderingService;
// using EShopOnAbp.CmskitService;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceDomainSharedModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule)
        // typeof(CatalogServiceApplicationContractsModule),
        // typeof(OrderingServiceApplicationContractsModule),
        // typeof(CmskitServiceApplicationContractsModule)
    )]
    public class AdminServiceApplicationContractsModule : AbpModule
    {
    }
}