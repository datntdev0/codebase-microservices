using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.Microservice.Configuration;
using datntdev.Microservice.EntityFrameworkCore;
using datntdev.Microservice.Migrator.DependencyInjection;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;

namespace datntdev.Microservice.Migrator;

[DependsOn(typeof(MicroserviceEntityFrameworkModule))]
public class MicroserviceMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public MicroserviceMigratorModule(MicroserviceEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(MicroserviceMigratorModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            MicroserviceConsts.ConnectionStringName
        );

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(
            typeof(IEventBus),
            () => IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            )
        );
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MicroserviceMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}
