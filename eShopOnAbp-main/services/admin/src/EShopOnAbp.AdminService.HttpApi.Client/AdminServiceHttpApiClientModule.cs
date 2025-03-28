using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceApplicationContractsModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(AbpSettingManagementHttpApiClientModule)
    )]
    public class AdminServiceHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AdminServiceApplicationContractsModule).Assembly,
                AdminServiceRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
