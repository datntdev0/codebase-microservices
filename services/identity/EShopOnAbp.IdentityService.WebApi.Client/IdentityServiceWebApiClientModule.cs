using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace EShopOnAbp.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceContractsModule),
        typeof(AbpIdentityHttpApiClientModule)
    )]
    public class IdentityServiceWebApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(IdentityServiceContractsModule).Assembly,
                IdentityServiceRemoteServiceConsts.RemoteServiceName
            );
        }
    }
}
