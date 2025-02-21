using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Zero;
using datntdev.Abp.Zero.Configuration;
using datntdev.Microservice.Authorization.Permissions;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Configuration;
using datntdev.Microservice.Localization;
using datntdev.Microservice.MultiTenancy;

namespace datntdev.Microservice;

[DependsOn(typeof(AbpZeroCoreModule), typeof(MicroserviceCoreSharedModule))]
public class MicroserviceCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);
        Configuration.Authorization.Providers.Add<PermissionProvider>();

        MicroserviceLocalizationConfigurer.Configure(Configuration.Localization);

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MicroserviceCoreModule).GetAssembly());
    }
}
