using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Abp.EntityFrameworkCore.Repositories;

public interface IRepositoryWithDbContext
{
    DbContext GetDbContext();

    Task<DbContext> GetDbContextAsync();
}