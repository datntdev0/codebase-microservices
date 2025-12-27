using datntdev.Microservices.Common.Modular;
using datntdev.Microservices.Common.Web.App.Application;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace datntdev.Microservices.ServiceDefaults.Hosting
{
    internal class ServiceBootstrap<TModule> where TModule : BaseModule
    {
        private readonly IEnumerable<BaseModule> _modules = CreateAllModuleInstances();

        public void ConfigureServices(IServiceCollection services, IConfigurationRoot configs)
        {
            _modules.ToList().ForEach(module => 
            {
                module.ConfigureServices(services, configs);
                RegisterProviderServices(module, services);
                RegisterManagerServices(module, services);
                RegisterFluentValidators(module, services);
                RegisterInjectableServices(module, services);
                RegisterApplicationPartAssembly(module, services);
            });

            // Register all assemblies for AutoMapper
            services.AddAutoMapper(_modules.Select(m => m.GetType().Assembly).ToArray());
        }

        public void Configure(IServiceProvider serviceProvider, IConfigurationRoot configs)
        {
            _modules.ToList().ForEach(module => module.Configure(serviceProvider, configs));
        }

        private static IEnumerable<BaseModule> CreateAllModuleInstances()
        {
            return FindDependedModuleTypesRecursively(typeof(TModule))
                .Append(typeof(TModule))
                .Select(Activator.CreateInstance)
                .Select(module => (BaseModule)module!);
        }

        private static IEnumerable<Type> FindDependedModuleTypesRecursively(Type moduleType)
        {
            if (!moduleType.GetTypeInfo().IsDefined(typeof(DependOnAttribute), true)) return [];

            var moduleTypes = moduleType.GetTypeInfo()
                .GetCustomAttributes(typeof(DependOnAttribute), true)
                .Cast<DependOnAttribute>()
                .SelectMany(x => x.DependedModuleTypes)
                .Distinct();

            return moduleTypes
                .SelectMany(FindDependedModuleTypesRecursively)
                .Concat(moduleTypes);
        }

        private static void RegisterInjectableServices(BaseModule module, IServiceCollection services)
        {
            var injectServiceTypes = module.GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass && !type.IsAbstract)
                 .Where(type => type.GetTypeInfo().CustomAttributes
                     .Any(att => att.AttributeType == typeof(InjectableServiceAttribute))
                 );

            foreach (var type in injectServiceTypes)
            {
                switch (type.GetTypeInfo().GetCustomAttribute<InjectableServiceAttribute>()?.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(type);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RegisterManagerServices(BaseModule module, IServiceCollection services)
        {
            var managerServiceTypes = module.GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass && !type.IsAbstract)
                 .Where(type => type.IsAssignableTo(typeof(BaseAppManager)));
            managerServiceTypes.ToList().ForEach(type => services.AddScoped(type));
        }

        private static void RegisterProviderServices(BaseModule module, IServiceCollection services)
        {
            var providerServiceTypes = module.GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass && !type.IsAbstract)
                 .Where(type => type.IsAssignableTo(typeof(BaseAppProvider)));
            providerServiceTypes.ToList().ForEach(type => services.AddSingleton(type));
        }

        private static void RegisterFluentValidators(BaseModule module, IServiceCollection services)
        {
            var validatorTypes = module.GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass && !type.IsAbstract)
                 .Where(type => type.IsAssignableTo(typeof(IValidator)));
            validatorTypes.ToList().ForEach(type => services.AddScoped(type));
        }

        private static void RegisterApplicationPartAssembly(BaseModule module, IServiceCollection services)
        {
            if (AppServiceStartup.ServiceType != Common.Constants.Enum.ServiceType.Microservice) return;

            services.AddControllers().ConfigureApplicationPartManager(manager =>
            {
                var assembly = module.GetType().Assembly;
                if (!manager.ApplicationParts.OfType<AssemblyPart>().Any(ap => ap.Assembly == assembly))
                {
                    manager.ApplicationParts.Add(new AssemblyPart(assembly));
                }
            });
        }
    }
}
