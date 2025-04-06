using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceContractsModule),
        typeof(AbpPermissionManagementHttpApiClientModule),
        typeof(AbpSettingManagementHttpApiClientModule)
    )]
    public class AdminServiceWebApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AdminServiceContractsModule).Assembly,
                AdminServiceRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
