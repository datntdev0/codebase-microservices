using Volo.Abp.Modularity;

namespace EShopOnAbp.BasketService;

[DependsOn(typeof(BasketServiceContractsModule))]
public class BasketServiceApplicationModule : AbpModule
{
}