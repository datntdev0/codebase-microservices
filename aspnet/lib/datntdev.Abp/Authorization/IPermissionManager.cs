﻿using System.Collections.Generic;
using System.Threading.Tasks;
using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Authorization
{
    /// <summary>
    /// Permission manager.
    /// </summary>
    public interface IPermissionManager
    {
        /// <summary>
        /// Gets <see cref="Permission"/> object with given <paramref name="name"/> or throws exception
        /// if there is no permission with given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        Permission GetPermission(string name);

        /// <summary>
        /// Gets <see cref="Permission"/> object with given <paramref name="name"/> or returns null
        /// if there is no permission with given <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        Permission GetPermissionOrNull(string name);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <param name="tenancyFilter">Can be passed false to disable tenancy filter.</param>
        IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <param name="tenancyFilter">Can be passed false to disable tenancy filter.</param>
        Task<IReadOnlyList<Permission>> GetAllPermissionsAsync(bool tenancyFilter = true);

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <param name="multiTenancySides">Multi-tenancy side to filter</param>
        IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides);
        
        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <param name="multiTenancySides">Multi-tenancy side to filter</param>
        Task<IReadOnlyList<Permission>> GetAllPermissionsAsync(MultiTenancySides multiTenancySides);
    }
}
