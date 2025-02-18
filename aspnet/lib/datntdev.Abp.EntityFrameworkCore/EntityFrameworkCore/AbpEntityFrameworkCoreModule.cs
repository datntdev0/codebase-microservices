using System;
using System.Reflection;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Dependency;
using datntdev.Abp.EntityFramework;
using datntdev.Abp.EntityFramework.Repositories;
using datntdev.Abp.EntityFrameworkCore.Configuration;
using datntdev.Abp.EntityFrameworkCore.Repositories;
using datntdev.Abp.EntityFrameworkCore.Uow;
using datntdev.Abp.Modules;
using datntdev.Abp.Orm;
using datntdev.Abp.Reflection;
using datntdev.Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;

namespace datntdev.Abp.EntityFrameworkCore;

/// <summary>
/// This module is used to implement "Data Access Layer" in EntityFramework.
/// </summary>
[DependsOn(typeof(AbpEntityFrameworkCommonModule))]
public class AbpEntityFrameworkCoreModule : AbpModule
{
    private readonly ITypeFinder _typeFinder;

    public AbpEntityFrameworkCoreModule(ITypeFinder typeFinder)
    {
        _typeFinder = typeFinder;
    }

    public override void PreInitialize()
    {
        IocManager.Register<IAbpEfCoreConfiguration, AbpEfCoreConfiguration>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(AbpEntityFrameworkCoreModule).GetAssembly());

        IocManager.IocContainer.Register(
            Component.For(typeof(IDbContextProvider<>))
                .ImplementedBy(typeof(UnitOfWorkDbContextProvider<>))
                .LifestyleTransient()
            );

        RegisterGenericRepositoriesAndMatchDbContexes();
    }

    private void RegisterGenericRepositoriesAndMatchDbContexes()
    {
        var dbContextTypes =
            _typeFinder.Find(type =>
            {
                var typeInfo = type.GetTypeInfo();
                return typeInfo.IsPublic &&
                       !typeInfo.IsAbstract &&
                       typeInfo.IsClass &&
                       typeof(AbpDbContext).IsAssignableFrom(type);
            });

        if (dbContextTypes.IsNullOrEmpty())
        {
            Logger.Warn("No class found derived from AbpDbContext.");
            return;
        }

        using (IScopedIocResolver scope = IocManager.CreateScope())
        {
            foreach (var dbContextType in dbContextTypes)
            {
                Logger.Debug("Registering DbContext: " + dbContextType.AssemblyQualifiedName);

                scope.Resolve<IEfGenericRepositoryRegistrar>().RegisterForDbContext(dbContextType, IocManager, EfCoreAutoRepositoryTypes.Default);

                IocManager.IocContainer.Register(
                    Component.For<ISecondaryOrmRegistrar>()
                        .Named(Guid.NewGuid().ToString("N"))
                        .Instance(new EfCoreBasedSecondaryOrmRegistrar(dbContextType, scope.Resolve<IDbContextEntityFinder>()))
                        .LifestyleTransient()
                );
            }

            scope.Resolve<IDbContextTypeMatcher>().Populate(dbContextTypes);
        }
    }
}