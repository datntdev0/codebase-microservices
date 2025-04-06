using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace EShopOnAbp.PaymentService
{
    [DependsOn(
        typeof(PaymentServiceContractsModule),
        typeof(AbpHttpClientModule))]
    public class PaymentServiceWebApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(PaymentServiceContractsModule).Assembly,
                PaymentServiceRemoteServiceConsts.RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PaymentServiceWebApiClientModule>();
            });

        }
    }
}
