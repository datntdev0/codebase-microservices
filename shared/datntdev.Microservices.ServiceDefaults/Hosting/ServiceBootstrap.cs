using datntdev.Microservices.Common.Modular;
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
            _modules.ToList().ForEach(module => module.ConfigureServices(services, configs));
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
    }
}
