﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using Castle.Core.Logging;

namespace datntdev.Abp.Modules
{
    /// <summary>
    /// This class must be implemented by all module definition classes.
    /// </summary>
    /// <remarks>
    /// A module definition class is generally located in its own assembly
    /// and implements some action in module events on application startup and shutdown.
    /// It also defines depended modules.
    /// </remarks>
    public abstract class AbpModule
    {
        /// <summary>
        /// Gets a reference to the IOC manager.
        /// </summary>
        public IIocManager IocManager { get; set; }

        /// <summary>
        /// Gets a reference to the ABP configuration.
        /// </summary>
        public IAbpStartupConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILogger Logger { get; set; }

        protected AbpModule()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// This is the first event called on application startup.
        /// Codes can be placed here to run before dependency injection registrations.
        /// </summary>
        public virtual void PreInitialize()
        {

        }

        /// <summary>
        /// This method is used to register dependencies for this module.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// This method is called lastly on application startup.
        /// </summary>
        public virtual void PostInitialize()
        {

        }

        /// <summary>
        /// This method is called when the application is being shutdown.
        /// </summary>
        public virtual void Shutdown()
        {

        }

        public virtual Assembly[] GetAdditionalAssemblies()
        {
            return new Assembly[0];
        }

        /// <summary>
        /// Checks if given type is an Abp module class.
        /// </summary>
        /// <param name="type">Type to check</param>
        public static bool IsAbpModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(AbpModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// Finds direct depended modules of a module (excluding given module).
        /// </summary>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsAbpModule(moduleType))
            {
                throw new AbpInitializationException("This type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.GetTypeInfo().IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetTypeInfo().GetCustomAttributes(typeof(DependsOnAttribute), true).Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }

        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGivenModule(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesRecursively(list, moduleType);
            list.AddIfNotContains(typeof(AbpKernelModule));
            return list;
        }

        private static void AddModuleAndDependenciesRecursively(List<Type> modules, Type module)
        {
            if (!IsAbpModule(module))
            {
                throw new AbpInitializationException("This type is not an ABP module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesRecursively(modules, dependedModule);
            }
        }
    }
}
