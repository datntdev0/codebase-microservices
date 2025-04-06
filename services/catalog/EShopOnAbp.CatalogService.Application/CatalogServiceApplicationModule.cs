using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Domain;
using EShopOnAbp.CatalogService.Products;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using EShopOnAbp.CatalogService.Application;

namespace EShopOnAbp.CatalogService;

[DependsOn(
    typeof(CatalogServiceContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpDddDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpMongoDbModule)
)]
public class CatalogServiceApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<CatalogServiceApplicationModule>(validate: true); });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.AutoEventSelectors.Add<Product>();
            options.EtoMappings.Add<Product, ProductEto>();
        });
        
        context.Services.AddMongoDbContext<CatalogServiceMongoDbContext>(options =>
        {
            options.AddDefaultRepositories();
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, MongoQuestionRepository>();
             */
        });
    }
}