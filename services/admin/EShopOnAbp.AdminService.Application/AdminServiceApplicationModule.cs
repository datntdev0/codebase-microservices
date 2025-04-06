using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;

namespace EShopOnAbp.AdminService
{
    [DependsOn(
        typeof(AdminServiceContractsModule),
        // Domain modules
        typeof(AbpSettingManagementDomainModule),
        typeof(AbpAuditLoggingDomainModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        // Application modules
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpSettingManagementApplicationModule)
    )]
    public class AdminServiceApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure AutoMapper
            context.Services.AddAutoMapperObjectMapper<AdminServiceApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AdminServiceApplicationModule>(validate: true);
            });

            // Configure Localization
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("sl", "sl", "Slovenščina"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("de-DE", "de-DE", "Deutsche"));
                options.Languages.Add(new LanguageInfo("es", "es", "Español"));
            });
        }
    }
}
