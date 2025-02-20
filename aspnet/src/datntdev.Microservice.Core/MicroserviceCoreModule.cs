using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Runtime.Security;
using datntdev.Abp.Timing;
using datntdev.Abp.Zero;
using datntdev.Abp.Zero.Configuration;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Configuration;
using datntdev.Microservice.Localization;
using datntdev.Microservice.MultiTenancy;
using datntdev.Microservice.Timing;

namespace datntdev.Microservice;

[DependsOn(typeof(AbpZeroCoreModule))]
public class MicroserviceCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Auditing.IsEnabledForAnonymousUsers = true;

        // Declare entity types
        Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
        Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
        Configuration.Modules.Zero().EntityTypes.User = typeof(User);

        MicroserviceLocalizationConfigurer.Configure(Configuration.Localization);

        // Enable this line to create a multi-tenant application.
        Configuration.MultiTenancy.IsEnabled = MicroserviceConsts.MultiTenancyEnabled;

        // Configure roles
        AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

        Configuration.Settings.Providers.Add<AppSettingProvider>();

        Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = MicroserviceConsts.DefaultPassPhrase;
        SimpleStringCipher.DefaultPassPhrase = MicroserviceConsts.DefaultPassPhrase;
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MicroserviceCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
    }
}
