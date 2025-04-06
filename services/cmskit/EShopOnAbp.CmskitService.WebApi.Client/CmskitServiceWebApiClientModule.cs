using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceContractsModule),
    typeof(AbpHttpClientModule),
    typeof(CmsKitHttpApiClientModule)
    )]
public class CmskitServiceWebApiClientModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FeatureConfigurer.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(typeof(CmskitServiceContractsModule).Assembly,
            CmskitServiceRemoteServiceConsts.RemoteServiceName);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmskitServiceWebApiClientModule>();
        });
    }
}
