using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.CmsKit;
using Volo.CmsKit.EntityFrameworkCore;

namespace EShopOnAbp.CmskitService;

[DependsOn(
    typeof(CmskitServiceContractsModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEntityFrameworkCorePostgreSqlModule),
    typeof(CmsKitDomainModule),
    typeof(CmsKitApplicationModule),
    typeof(CmsKitEntityFrameworkCoreModule)
)]
public class CmskitServiceApplicationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        EntityFrameworkCore.CmskitServiceEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmskitServiceApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmskitServiceApplicationModule>(validate: true);
        });

        context.Services.AddAbpDbContext<EntityFrameworkCore.CmskitServiceDbContext>(options =>
        {
            options.ReplaceDbContext<EntityFrameworkCore.ICmskitServiceDbContext>();
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also CmskitServiceMigrationsDbContextFactory for EF Core tooling. */
            options.UseNpgsql(b =>
            {
                b.MigrationsHistoryTable("__CmskitService_Migrations");
            });
        });
    }
}
