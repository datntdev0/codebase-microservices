using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace datntdev.Microservice.EntityFrameworkCore;

public static class MicroserviceDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<MicroserviceDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<MicroserviceDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
