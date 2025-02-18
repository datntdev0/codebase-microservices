using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using datntdev.Abp.Configuration.Startup;
using Microsoft.Extensions.Caching.Memory;

namespace datntdev.Abp.Runtime.Caching.Configuration
{
    public class CachingConfiguration : ICachingConfiguration
    {
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToImmutableList(); }
        }

        public MemoryCacheOptions MemoryCacheOptions { get; set; }
        
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;

            _configurators = new List<ICacheConfigurator>();
        }

        public void ConfigureAll(Action<ICacheOptions> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICacheOptions> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}