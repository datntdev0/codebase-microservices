using Volo.Abp.Modularity;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceApplicationModule),
        typeof(AdminServiceDomainTestModule)
        )]
    public class AdminServiceApplicationTestModule : AbpModule
    {

    }
}