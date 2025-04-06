using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace EShopOnAbp.OrderingService.EntityFrameworkCore;

[DependsOn(
    typeof(OrderingServiceApplicationModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule)
)]
public class OrderingServiceEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        OrderingServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}