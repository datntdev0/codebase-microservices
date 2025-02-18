using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;

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
