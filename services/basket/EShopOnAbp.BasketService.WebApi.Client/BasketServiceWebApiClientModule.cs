using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.BasketService
{
    [DependsOn(
        typeof(BasketServiceContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class BasketServiceWebApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Basket";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(BasketServiceContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BasketServiceWebApiClientModule>();
            });
        }
    }
}
