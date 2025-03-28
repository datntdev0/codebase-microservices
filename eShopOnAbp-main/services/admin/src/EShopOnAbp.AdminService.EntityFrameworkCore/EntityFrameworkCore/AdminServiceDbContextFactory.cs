using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EShopOnAbp.AdminService.EntityFrameworkCore
{
    public class AdminServiceDbContextFactory : IDesignTimeDbContextFactory<AdminServiceDbContext>
    {
        public AdminServiceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AdminServiceDbContext>()
                .UseNpgsql(GetConnectionStringFromConfiguration(), b =>
                {
                    b.MigrationsHistoryTable("__AdminService_Migrations");
                });

            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return new AdminServiceDbContext(builder.Options);
        }

        private static string GetConnectionStringFromConfiguration()
        {
            return BuildConfiguration()
                .GetConnectionString(AdminServiceDbProperties.ConnectionStringName);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        $"..{Path.DirectorySeparatorChar}EShopOnAbp.AdminService.HttpApi.Host"
                    )
                )
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
