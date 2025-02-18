using datntdev.Abp.Application.Features;
using datntdev.Abp.Auditing;
using datntdev.Abp.BackgroundJobs;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.DynamicEntityProperties;
using datntdev.Abp.EntityHistory;
using datntdev.Abp.Localization;
using datntdev.Abp.Modules;
using datntdev.Abp.Notifications;
using datntdev.Abp.PlugIns;
using datntdev.Abp.Reflection;
using datntdev.Abp.Resources.Embedded;
using datntdev.Abp.Runtime.Caching.Configuration;
using datntdev.Abp.Runtime.Validation;
using datntdev.Abp.Webhooks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace datntdev.Abp.Dependency.Installers
{
    public class AbpCoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>().ImplementedBy<UnitOfWorkDefaultOptions>().LifestyleSingleton(),
                Component.For<IAbpValidationDefaultOptions, AbpValidationDefaultOptions>().ImplementedBy<AbpValidationDefaultOptions>().LifestyleSingleton(),
                Component.For<IAbpAuditingDefaultOptions, AbpAuditingDefaultOptions>().ImplementedBy<AbpAuditingDefaultOptions>().LifestyleSingleton(),
                Component.For<INavigationConfiguration, NavigationConfiguration>().ImplementedBy<NavigationConfiguration>().LifestyleSingleton(),
                Component.For<ILocalizationConfiguration, LocalizationConfiguration>().ImplementedBy<LocalizationConfiguration>().LifestyleSingleton(),
                Component.For<IAuthorizationConfiguration, AuthorizationConfiguration>().ImplementedBy<AuthorizationConfiguration>().LifestyleSingleton(),
                Component.For<IValidationConfiguration, ValidationConfiguration>().ImplementedBy<ValidationConfiguration>().LifestyleSingleton(),
                Component.For<IFeatureConfiguration, FeatureConfiguration>().ImplementedBy<FeatureConfiguration>().LifestyleSingleton(),
                Component.For<ISettingsConfiguration, SettingsConfiguration>().ImplementedBy<SettingsConfiguration>().LifestyleSingleton(),
                Component.For<IModuleConfigurations, ModuleConfigurations>().ImplementedBy<ModuleConfigurations>().LifestyleSingleton(),
                Component.For<IEventBusConfiguration, EventBusConfiguration>().ImplementedBy<EventBusConfiguration>().LifestyleSingleton(),
                Component.For<IMultiTenancyConfig, MultiTenancyConfig>().ImplementedBy<MultiTenancyConfig>().LifestyleSingleton(),
                Component.For<ICachingConfiguration, CachingConfiguration>().ImplementedBy<CachingConfiguration>().LifestyleSingleton(),
                Component.For<IAuditingConfiguration, AuditingConfiguration>().ImplementedBy<AuditingConfiguration>().LifestyleSingleton(),
                Component.For<IBackgroundJobConfiguration, BackgroundJobConfiguration>().ImplementedBy<BackgroundJobConfiguration>().LifestyleSingleton(),
                Component.For<INotificationConfiguration, NotificationConfiguration>().ImplementedBy<NotificationConfiguration>().LifestyleSingleton(),
                Component.For<IEmbeddedResourcesConfiguration, EmbeddedResourcesConfiguration>().ImplementedBy<EmbeddedResourcesConfiguration>().LifestyleSingleton(),
                Component.For<IAbpStartupConfiguration, AbpStartupConfiguration>().ImplementedBy<AbpStartupConfiguration>().LifestyleSingleton(),
                Component.For<IEntityHistoryConfiguration, EntityHistoryConfiguration>().ImplementedBy<EntityHistoryConfiguration>().LifestyleSingleton(),
                Component.For<ITypeFinder, TypeFinder>().ImplementedBy<TypeFinder>().LifestyleSingleton(),
                Component.For<IAbpPlugInManager, AbpPlugInManager>().ImplementedBy<AbpPlugInManager>().LifestyleSingleton(),
                Component.For<IAbpModuleManager, AbpModuleManager>().ImplementedBy<AbpModuleManager>().LifestyleSingleton(),
                Component.For<IAssemblyFinder, AbpAssemblyFinder>().ImplementedBy<AbpAssemblyFinder>().LifestyleSingleton(),
                Component.For<ILocalizationManager, LocalizationManager>().ImplementedBy<LocalizationManager>().LifestyleSingleton(),
                Component.For<IWebhooksConfiguration, WebhooksConfiguration>().ImplementedBy<WebhooksConfiguration>().LifestyleSingleton(),
                Component.For<IDynamicEntityPropertyDefinitionContext, DynamicEntityPropertyDefinitionContext>().ImplementedBy<DynamicEntityPropertyDefinitionContext>().LifestyleTransient(),
                Component.For<IDynamicEntityPropertyConfiguration, DynamicEntityPropertyConfiguration>().ImplementedBy<DynamicEntityPropertyConfiguration>().LifestyleSingleton()
            );
        }
    }
}
