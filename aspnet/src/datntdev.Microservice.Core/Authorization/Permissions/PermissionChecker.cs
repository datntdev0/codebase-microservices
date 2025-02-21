using datntdev.Abp.Authorization;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;

namespace datntdev.Microservice.Authorization.Permissions;

public class PermissionChecker : PermissionChecker<Role, User>
{
    public PermissionChecker(UserManager userManager)
        : base(userManager)
    {
    }
}
