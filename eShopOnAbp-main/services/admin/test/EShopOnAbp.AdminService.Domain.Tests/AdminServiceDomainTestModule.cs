using EShopOnAbp.AdminService.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceEntityFrameworkCoreTestModule)
        )]
    public class AdminServiceDomainTestModule : AbpModule
    {

    }
}