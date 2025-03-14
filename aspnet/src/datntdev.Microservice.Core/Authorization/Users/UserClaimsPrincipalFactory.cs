﻿using datntdev.Abp.Authorization;
using datntdev.Abp.Domain.Uow;
using datntdev.Microservice.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace datntdev.Microservice.Authorization.Users;

public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
{
    public UserClaimsPrincipalFactory(
        UserManager userManager,
        RoleManager roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        IUnitOfWorkManager unitOfWorkManager)
        : base(
              userManager,
              roleManager,
              optionsAccessor,
              unitOfWorkManager)
    {
    }
}
