using System.Threading.Tasks;
using datntdev.Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Abp.EntityFrameworkCore;

public interface IDbContextProvider<TDbContext>
    where TDbContext : DbContext
{
    Task<TDbContext> GetDbContextAsync();

    Task<TDbContext> GetDbContextAsync(MultiTenancySides? multiTenancySide);

    TDbContext GetDbContext();

    TDbContext GetDbContext(MultiTenancySides? multiTenancySide);
}