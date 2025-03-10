using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.EntityFrameworkCore.Extensions;
using datntdev.Abp.Transactions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace datntdev.Abp.EntityFrameworkCore.Uow;

public class DbContextEfCoreTransactionStrategy : IEfCoreTransactionStrategy, ITransientDependency
{
    protected UnitOfWorkOptions Options { get; private set; }

    protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }

    public DbContextEfCoreTransactionStrategy()
    {
        ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>(System.StringComparer.OrdinalIgnoreCase);
    }

    public void InitOptions(UnitOfWorkOptions options)
    {
        Options = options;
    }

    public async Task<DbContext> CreateDbContextAsync<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
    {
        DbContext dbContext;

        var activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
        if (activeTransaction == null)
        {
            dbContext = dbContextResolver.Resolve<TDbContext>(connectionString, null);

            var dbTransaction = await dbContext.Database.BeginTransactionAsync(
                    (Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());

            activeTransaction = new ActiveTransactionInfo(dbTransaction, dbContext);
            ActiveTransactions[connectionString] = activeTransaction;
        }
        else
        {
            dbContext = dbContextResolver.Resolve<TDbContext>(
                connectionString,
                activeTransaction.DbContextTransaction.GetDbTransaction().Connection
            );

            if (dbContext.HasRelationalTransactionManager())
            {
                await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction());
            }
            else
            {
                await dbContext.Database.BeginTransactionAsync();
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);
        }

        return dbContext;
    }

    public void Commit()
    {
        foreach (var activeTransaction in ActiveTransactions.Values)
        {
            activeTransaction.DbContextTransaction.Commit();

            foreach (var dbContext in activeTransaction.AttendedDbContexts)
            {
                if (dbContext.HasRelationalTransactionManager())
                {
                    continue; //Relational databases use the shared transaction
                }

                dbContext.Database.CommitTransaction();
            }
        }
    }

    public void Dispose(IIocResolver iocResolver)
    {
        foreach (var activeTransaction in ActiveTransactions.Values)
        {
            activeTransaction.DbContextTransaction.Dispose();

            foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
            {
                iocResolver.Release(attendedDbContext);
            }

            iocResolver.Release(activeTransaction.StarterDbContext);
        }

        ActiveTransactions.Clear();
    }

    public DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext
    {
        DbContext dbContext;

        var activeTransaction = ActiveTransactions.GetOrDefault(connectionString);
        if (activeTransaction == null)
        {
            dbContext = dbContextResolver.Resolve<TDbContext>(connectionString, null);

            var dbTransaction = dbContext.Database.BeginTransaction(
                    (Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());

            activeTransaction = new ActiveTransactionInfo(dbTransaction, dbContext);
            ActiveTransactions[connectionString] = activeTransaction;
        }
        else
        {
            dbContext = dbContextResolver.Resolve<TDbContext>(
                connectionString,
                activeTransaction.DbContextTransaction.GetDbTransaction().Connection
            );

            if (dbContext.HasRelationalTransactionManager())
            {
                dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
            }
            else
            {
                dbContext.Database.BeginTransaction();
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);
        }

        return dbContext;
    }
}