using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace datntdev.Microservices.Common.Configuration
{
    public static class AppConfiguration
    {
        private static IConfigurationRoot _configurationCache = default!;

        public static IConfigurationRoot Get(IHostEnvironment env)
        {
            if (_configurationCache != null) return _configurationCache;

            var envName = env.EnvironmentName;

            _configurationCache = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddUserSecrets(Assembly.GetEntryAssembly()!, optional: true)
                .AddEnvironmentVariables()
                .Build();

            return _configurationCache;
        }
    }
}
