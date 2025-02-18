using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using datntdev.Microservice.EntityFrameworkCore;
using datntdev.Microservice.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace datntdev.Microservice.Web.Tests;

[DependsOn(
    typeof(MicroserviceWebMvcModule),
    typeof(AbpAspNetCoreTestBaseModule)
)]
public class MicroserviceWebTestModule : AbpModule
{
    public MicroserviceWebTestModule(MicroserviceEntityFrameworkModule abpProjectNameEntityFrameworkModule)
    {
        abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(MicroserviceWebTestModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        IocManager.Resolve<ApplicationPartManager>()
            .AddApplicationPartsIfNotAddedBefore(typeof(MicroserviceWebMvcModule).Assembly);
    }
}