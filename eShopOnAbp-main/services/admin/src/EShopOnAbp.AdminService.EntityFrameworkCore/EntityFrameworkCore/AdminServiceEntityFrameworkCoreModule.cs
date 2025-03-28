using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace EShopOnAbp.AdminService.EntityFrameworkCore
{
    [DependsOn(
        typeof(AdminServiceDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(BlobStoringDatabaseEntityFrameworkCoreModule)
        )]
    public class AdminServiceEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AdminServiceDbContext>(options =>
            {
                options.ReplaceDbContext<IPermissionManagementDbContext>();
                options.ReplaceDbContext<ISettingManagementDbContext>();
                options.ReplaceDbContext<IAuditLoggingDbContext>();
                options.ReplaceDbContext<IBlobStoringDbContext>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure<AdminServiceDbContext>(c =>
                {
                    c.UseNpgsql(b =>
                    {
                        b.MigrationsHistoryTable("__AdminService_Migrations");
                    });
                });
            });
        }
    }
}
