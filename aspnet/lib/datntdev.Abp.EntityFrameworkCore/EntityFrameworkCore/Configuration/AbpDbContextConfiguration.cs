using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using datntdev.Abp.Domain.Entities;

namespace datntdev.Abp.EntityFrameworkCore.Configuration;

public class AbpDbContextConfiguration<TDbContext>
    where TDbContext : DbContext
{
    public string ConnectionString { get; set; }

    public DbConnection ExistingConnection { get; set; }

    public DbContextOptionsBuilder<TDbContext> DbContextOptions { get; }

    public AbpDbContextConfiguration(string connectionString, DbConnection existingConnection)
    {
        ConnectionString = connectionString;
        ExistingConnection = existingConnection;

        DbContextOptions = new DbContextOptionsBuilder<TDbContext>();
    }
}