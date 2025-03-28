using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule)
        )]
    public class AdminServiceHttpApiModule : AbpModule
    {

    }
}
