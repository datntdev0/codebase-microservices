using datntdev.Abp.EntityFrameworkCore.Configuration;
using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Zero.EntityFrameworkCore;
using datntdev.Microservice.EntityFrameworkCore.Seed;

namespace datntdev.Microservice.EntityFrameworkCore;

[DependsOn(
    typeof(MicroserviceCoreModule),
    typeof(AbpZeroCoreEntityFrameworkCoreModule))]
public class MicroserviceEntityFrameworkModule : AbpModule
{
    /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
    public bool SkipDbContextRegistration { get; set; }

    public bool SkipDbSeed { get; set; }

    public override void PreInitialize()
    {
        if (!SkipDbContextRegistration)
        {
            Configuration.Modules.AbpEfCore().AddDbContext<MicroserviceDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    MicroserviceDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    MicroserviceDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MicroserviceEntityFrameworkModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        if (!SkipDbSeed)
        {
            SeedHelper.SeedHostDb(IocManager);
        }
    }
}
