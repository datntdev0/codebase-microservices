﻿using System.Threading.Tasks;
using datntdev.Abp.Domain.Uow;

namespace datntdev.Abp.MultiTenancy
{
    /// <summary>
    /// Extends <see cref="IConnectionStringResolver"/> to
    /// get connection string for given tenant.
    /// </summary>
    public interface IDbPerTenantConnectionStringResolver : IConnectionStringResolver
    {
        /// <summary>
        /// Gets the connection string for given args.
        /// </summary>
        string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args);

        /// <summary>
        /// Gets the connection string for given args.
        /// </summary>
        Task<string> GetNameOrConnectionStringAsync(DbPerTenantConnectionStringResolveArgs args);
    }
}
