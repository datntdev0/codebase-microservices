﻿using System.Linq;
using System.Reflection;
using datntdev.Abp.Application.Features;
using datntdev.Abp.Auditing;
using datntdev.Abp.Authorization.Roles;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization;
using datntdev.Abp.Localization.Dictionaries;
using datntdev.Abp.Localization.Dictionaries.Xml;
using datntdev.Abp.Modules;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Reflection;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using CastleNs = Castle;

namespace datntdev.Abp.Zero
{
    /// <summary>
    /// ABP zero core module.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpZeroCommonModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.RegisterIfNot<IAbpZeroEntityTypes, AbpZeroEntityTypes>(); //Registered on services.AddAbpIdentity() for Abp.Core.

            IocManager.Register<IRoleManagementConfig, RoleManagementConfig>();
            IocManager.Register<IUserManagementConfig, UserManagementConfig>();
            IocManager.Register<ILanguageManagementConfig, LanguageManagementConfig>();
            IocManager.Register<IAbpZeroConfig, AbpZeroConfig>();

            Configuration.ReplaceService<ITenantStore, TenantStore>(DependencyLifeStyle.Transient);

            Configuration.Settings.Providers.Add<AbpZeroSettingProvider>();

            IocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            AddIgnoredTypes();
        }

        public override void Initialize()
        {
            FillMissingEntityTypes();

            IocManager.Register<IMultiTenantLocalizationDictionary, MultiTenantLocalizationDictionary>(DependencyLifeStyle.Transient);
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroCommonModule).GetAssembly());

            RegisterTenantCache();
        }

        private void Kernel_ComponentRegistered(string key, CastleNs.MicroKernel.IHandler handler)
        {
            if (typeof(IAbpZeroFeatureValueStore).IsAssignableFrom(handler.ComponentModel.Implementation) && !IocManager.IsRegistered<IAbpZeroFeatureValueStore>())
            {
                IocManager.IocContainer.Register(
                    Component.For<IAbpZeroFeatureValueStore>().ImplementedBy(handler.ComponentModel.Implementation).Named("AbpZeroFeatureValueStore").LifestyleTransient()
                    );
            }
        }

        private void AddIgnoredTypes()
        {
            var ignoredTypes = new[]
            {
                typeof(AuditLog)
            };

            foreach (var ignoredType in ignoredTypes)
            {
                Configuration.EntityHistory.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }

        private void FillMissingEntityTypes()
        {
            using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
            {
                if (entityTypes.Object.User != null &&
                    entityTypes.Object.Role != null &&
                    entityTypes.Object.Tenant != null)
                {
                    return;
                }

                using (var typeFinder = IocManager.ResolveAsDisposable<ITypeFinder>())
                {
                    var types = typeFinder.Object.FindAll();
                    entityTypes.Object.Tenant = types.FirstOrDefault(t => typeof(AbpTenantBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                    entityTypes.Object.Role = types.FirstOrDefault(t => typeof(AbpRoleBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                    entityTypes.Object.User = types.FirstOrDefault(t => typeof(AbpUserBase).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract);
                }
            }
        }

        private void RegisterTenantCache()
        {
            if (IocManager.IsRegistered<ITenantCache>())
            {
                return;
            }

            using (var entityTypes = IocManager.ResolveAsDisposable<IAbpZeroEntityTypes>())
            {
                var implType = typeof (TenantCache<,>)
                    .MakeGenericType(entityTypes.Object.Tenant, entityTypes.Object.User);

                IocManager.Register(typeof (ITenantCache), implType, DependencyLifeStyle.Transient);
            }
        }
    }
}

