using datntdev.Abp.Authorization;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Configuration;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Zero.Configuration;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.MultiTenancy;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservice.Authorization;

public class LogInManager : AbpLogInManager<Tenant, Role, User>
{
    public LogInManager(
        UserManager userManager,
        IMultiTenancyConfig multiTenancyConfig,
        IRepository<Tenant> tenantRepository,
        IUnitOfWorkManager unitOfWorkManager,
        ISettingManager settingManager,
        IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
        IUserManagementConfig userManagementConfig,
        IIocResolver iocResolver,
        IPasswordHasher<User> passwordHasher,
        RoleManager roleManager,
        UserClaimsPrincipalFactory claimsPrincipalFactory)
        : base(
              userManager,
              multiTenancyConfig,
              tenantRepository,
              unitOfWorkManager,
              settingManager,
              userLoginAttemptRepository,
              userManagementConfig,
              iocResolver,
              passwordHasher,
              roleManager,
              claimsPrincipalFactory)
    {
    }
}
