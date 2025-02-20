using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization.Dictionaries;
using datntdev.Abp.Localization.Dictionaries.Xml;
using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Threading.BackgroundWorkers;
using datntdev.Abp.Zero.Configuration;

namespace datntdev.Abp.Zero;

[DependsOn(typeof(AbpZeroCommonModule))]
public class AbpZeroCoreModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Localization.Sources.Add(
            new DictionaryBasedLocalizationSource(
                AbpZeroConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(AbpZeroCoreModule).GetAssembly()
                )
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpZeroCoreModule).GetAssembly());
        RegisterUserTokenExpirationWorker();
    }

    public override void PostInitialize()
    {
        if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
            {
                var implType = typeof(UserTokenExpirationWorker<,>)
                    .MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);
                var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workerManager.Add(IocManager.Resolve(implType) as IBackgroundWorker);
            }
        }
    }

    private void RegisterUserTokenExpirationWorker()
    {
        using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
        {
            var implType = typeof(UserTokenExpirationWorker<,>)
                .MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);
            IocManager.Register(implType);
        }
    }
}