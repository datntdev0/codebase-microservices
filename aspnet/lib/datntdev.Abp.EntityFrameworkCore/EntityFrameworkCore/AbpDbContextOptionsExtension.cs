using System.Collections.Generic;
using System.Linq;
using datntdev.Abp.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace datntdev.Abp.EntityFrameworkCore;

public class AbpDbContextOptionsExtension : IDbContextOptionsExtension
{
    public void ApplyServices(IServiceCollection services)
    {
        var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(ICompiledQueryCacheKeyGenerator));
        if (serviceDescriptor != null && serviceDescriptor.ImplementationType != null)
        {
            services.Remove(serviceDescriptor);
            services.AddScoped(serviceDescriptor.ImplementationType);
            services.Add(ServiceDescriptor.Scoped<ICompiledQueryCacheKeyGenerator>(provider =>
                ActivatorUtilities.CreateInstance<AbpCompiledQueryCacheKeyGenerator>(provider,
                    provider.GetRequiredService(serviceDescriptor.ImplementationType)
                        .As<ICompiledQueryCacheKeyGenerator>())));
        }

        services.Replace(ServiceDescriptor.Scoped<IAsyncQueryProvider, AbpEntityQueryProvider>());
        services.AddSingleton<AbpEfCoreCurrentDbContext>();
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info => new AbpOptionsExtensionInfo(this);

    private class AbpOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public AbpOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => other is AbpOptionsExtensionInfo;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override string LogFragment => "AbpOptionsExtension";
    }
}