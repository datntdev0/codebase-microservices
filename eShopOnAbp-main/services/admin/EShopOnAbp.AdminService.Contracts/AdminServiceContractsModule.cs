using EShopOnAbp.AdminService.Localization;
using Volo.Abp.AuditLogging;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

// using EShopOnAbp.CatalogService;
// using EShopOnAbp.OrderingService;
// using EShopOnAbp.CmskitService;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AbpPermissionManagementDomainSharedModule),
        typeof(AbpSettingManagementDomainSharedModule),
        typeof(AbpAuditLoggingDomainSharedModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule)
        // typeof(CatalogServiceContractsModule),
        // typeof(OrderingServiceApplicationContractsModule),
        // typeof(CmskitServiceApplicationContractsModule)
    )]
    public class AdminServiceContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AdminServiceContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AdminServiceResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/AdminService");

                options.DefaultResourceType = typeof(AdminServiceResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("AdminService", typeof(AdminServiceResource));
            });
        }
    }
}