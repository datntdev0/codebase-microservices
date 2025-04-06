using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.CatalogService
{
    [DependsOn(
        typeof(CatalogServiceContractsModule),
        typeof(AbpHttpClientModule)
    )]
    public class CatalogServiceWebApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Catalog";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(CatalogServiceContractsModule).Assembly,
                RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CatalogServiceWebApiClientModule>();
            });
        }
    }
}
