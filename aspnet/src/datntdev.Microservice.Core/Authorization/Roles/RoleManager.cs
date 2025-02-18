using datntdev.Abp.Authorization;
using datntdev.Abp.Authorization.Roles;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Organizations;
using datntdev.Abp.Runtime.Caching;
using datntdev.Abp.Zero.Configuration;
using datntdev.Microservice.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace datntdev.Microservice.Authorization.Roles;

public class RoleManager : AbpRoleManager<Role, User>
{
    public RoleManager(
        RoleStore store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<AbpRoleManager<Role, User>> logger,
        IPermissionManager permissionManager,
        ICacheManager cacheManager,
        IUnitOfWorkManager unitOfWorkManager,
        IRoleManagementConfig roleManagementConfig,
        IRepository<OrganizationUnit, long> organizationUnitRepository,
        IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository)
        : base(
              store,
              roleValidators,
              keyNormalizer,
              errors, logger,
              permissionManager,
              cacheManager,
              unitOfWorkManager,
              roleManagementConfig,
            organizationUnitRepository,
            organizationUnitRoleRepository)
    {
    }
}
