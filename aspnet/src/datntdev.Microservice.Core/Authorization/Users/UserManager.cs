using datntdev.Abp.Authorization;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Configuration;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Organizations;
using datntdev.Abp.Runtime.Caching;
using datntdev.Microservice.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace datntdev.Microservice.Authorization.Users;

public class UserManager : AbpUserManager<Role, User>
{
    public UserManager(
      RoleManager roleManager,
      UserStore store,
      IOptions<IdentityOptions> optionsAccessor,
      IPasswordHasher<User> passwordHasher,
      IEnumerable<IUserValidator<User>> userValidators,
      IEnumerable<IPasswordValidator<User>> passwordValidators,
      ILookupNormalizer keyNormalizer,
      IdentityErrorDescriber errors,
      IServiceProvider services,
      ILogger<UserManager<User>> logger,
      IPermissionManager permissionManager,
      IUnitOfWorkManager unitOfWorkManager,
      ICacheManager cacheManager,
      IRepository<OrganizationUnit, long> organizationUnitRepository,
      IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
      IOrganizationUnitSettings organizationUnitSettings,
      ISettingManager settingManager,
      IRepository<UserLogin, long> userLoginRepository)
      : base(
          roleManager,
          store,
          optionsAccessor,
          passwordHasher,
          userValidators,
          passwordValidators,
          keyNormalizer,
          errors,
          services,
          logger,
          permissionManager,
          unitOfWorkManager,
          cacheManager,
          organizationUnitRepository,
          userOrganizationUnitRepository,
          organizationUnitSettings,
          settingManager,
          userLoginRepository)
    {
    }
}
