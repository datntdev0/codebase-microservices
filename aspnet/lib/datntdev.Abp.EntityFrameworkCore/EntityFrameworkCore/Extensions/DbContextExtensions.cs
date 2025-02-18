using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace datntdev.Abp.EntityFrameworkCore.Extensions;

public static class DbContextExtensions
{
    public static bool HasRelationalTransactionManager(this DbContext dbContext)
    {
        return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
    }
}