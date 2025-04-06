using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.OrderingService;

[DependsOn(
    typeof(OrderingServiceContractsModule),
    typeof(AbpHttpClientModule))]
public class OrderingServiceWebApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Ordering";
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(OrderingServiceContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<OrderingServiceWebApiClientModule>();
        });
    }
}