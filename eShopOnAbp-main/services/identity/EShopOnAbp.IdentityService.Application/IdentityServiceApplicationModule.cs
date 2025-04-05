using EShopOnAbp.IdentityService.EntityFrameworkCore;
using EShopOnAbp.IdentityService.Keycloak;
using EShopOnAbp.IdentityService.Keycloak.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EShopOnAbp.IdentityService
{
    [DependsOn(
        typeof(IdentityServiceContractsModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityDomainModule),
        typeof(AbpBackgroundJobsModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
        typeof(AbpIdentityEntityFrameworkCoreModule)
    )]
    public class IdentityServiceApplicationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            IdentityServiceEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            
            context.Services.AddAutoMapperObjectMapper<IdentityServiceApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityServiceApplicationModule>(validate: true);
            });
            
            Configure<KeycloakClientOptions>(options =>
                {
                    options.Url = configuration["Keycloak:url"];
                    options.AdminUserName = configuration["Keycloak:adminUsername"];
                    options.AdminPassword = configuration["Keycloak:adminPassword"];
                    options.RealmName = configuration["Keycloak:realmName"];
                }
            );

            // Database configuration
            context.Services.AddAbpDbContext<IdentityServiceDbContext>(options =>
            {
                options.ReplaceDbContext<IIdentityDbContext>();
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<IdentityServiceDbContext>(c =>
                {
                    c.UseNpgsql(b => { b.MigrationsHistoryTable("__IdentityService_Migrations"); });
                });
            });
        }
    }
}