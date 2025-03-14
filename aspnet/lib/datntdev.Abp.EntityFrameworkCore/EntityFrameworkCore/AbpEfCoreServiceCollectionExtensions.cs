using System;
using datntdev.Abp.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Abp.EntityFrameworkCore;

public static class AbpEfCoreServiceCollectionExtensions
{
    public static void AddAbpDbContext<TDbContext>(
        this IServiceCollection services,
        Action<AbpDbContextConfiguration<TDbContext>> action)
        where TDbContext : DbContext
    {
        services.AddSingleton(
            typeof(IAbpDbContextConfigurer<TDbContext>),
            new AbpDbContextConfigurerAction<TDbContext>(action)
        );
    }
}