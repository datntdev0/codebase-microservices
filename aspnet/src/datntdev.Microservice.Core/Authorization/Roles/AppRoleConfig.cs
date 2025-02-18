using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Zero.Configuration;

namespace datntdev.Microservice.Authorization.Roles;

public static class AppRoleConfig
{
    public static void Configure(IRoleManagementConfig roleManagementConfig)
    {
        // Static host roles

        roleManagementConfig.StaticRoles.Add(
            new StaticRoleDefinition(
                StaticRoleNames.Host.Admin,
                MultiTenancySides.Host
            )
        );

        // Static tenant roles

        roleManagementConfig.StaticRoles.Add(
            new StaticRoleDefinition(
                StaticRoleNames.Tenants.Admin,
                MultiTenancySides.Tenant
            )
        );
    }
}
