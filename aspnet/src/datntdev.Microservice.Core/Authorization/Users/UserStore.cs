using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Organizations;
using datntdev.Microservice.Authorization.Roles;

namespace datntdev.Microservice.Authorization.Users;

public class UserStore : AbpUserStore<Role, User>
{
    public UserStore(
        IUnitOfWorkManager unitOfWorkManager,
        IRepository<User, long> userRepository,
        IRepository<Role> roleRepository,
        IRepository<UserRole, long> userRoleRepository,
        IRepository<UserLogin, long> userLoginRepository,
        IRepository<UserClaim, long> userClaimRepository,
        IRepository<UserPermissionSetting, long> userPermissionSettingRepository,
        IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
        IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
        IRepository<UserToken, long> userTokenRepository
    )
        : base(unitOfWorkManager,
              userRepository,
              roleRepository,
              userRoleRepository,
              userLoginRepository,
              userClaimRepository,
              userPermissionSettingRepository,
              userOrganizationUnitRepository,
              organizationUnitRoleRepository,
              userTokenRepository
        )
    {
    }
}
