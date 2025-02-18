using datntdev.Abp.Domain.Uow;
using datntdev.Abp.EntityFramework;

namespace datntdev.Abp.EntityFrameworkCore;

public class DbContextTypeMatcher : DbContextTypeMatcher<AbpDbContext>
{
    public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        : base(currentUnitOfWorkProvider)
    {
    }
}