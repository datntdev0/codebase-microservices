using datntdev.Abp.Domain.Uow;
using datntdev.Abp.EntityFrameworkCore;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Zero.EntityFrameworkCore;

namespace datntdev.Microservice.EntityFrameworkCore;

public class AbpZeroDbMigrator : AbpZeroDbMigrator<MicroserviceDbContext>
{
    public AbpZeroDbMigrator(
        IUnitOfWorkManager unitOfWorkManager,
        IDbPerTenantConnectionStringResolver connectionStringResolver,
        IDbContextResolver dbContextResolver)
        : base(
            unitOfWorkManager,
            connectionStringResolver,
            dbContextResolver)
    {
    }
}
