using datntdev.Microservices.Common.Extensions;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions.Models;
using System.Collections.Immutable;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions
{
    public class PermissionProvider : BaseAppProvider
    {
        private readonly ImmutableDictionary<AppPermission, PermissionModel> _permissions;

        public PermissionProvider() 
        { 
            _permissions = LoadPermissions();
        }

        public PermissionModel[] GetAllPermissions(MultiTenancySide? tenancySide = null)
        {
            return _permissions.Values
                .WhereIf(tenancySide != null, x => (x.TenancySide & tenancySide) == tenancySide)
                .ToArray();
        }

        public AppPermission[] GetAllHostAppPermissions()
        {
            return GetAllPermissions(MultiTenancySide.Host)
                .Select(x => x.Permission).ToArray();
        }

        public AppPermission[] GetAllTenantAppPermissions()
        {
            return GetAllPermissions(MultiTenancySide.Tenant)
                .Select(x => x.Permission).ToArray();
        }

        private static ImmutableDictionary<AppPermission, PermissionModel> LoadPermissions()
        {
            var permissions = new Dictionary<AppPermission, PermissionModel>
            {
                { 
                    AppPermission.MultiTenancy, 
                    new PermissionModel(
                        permissionName: "Multi-tenancy management",
                        permission: AppPermission.MultiTenancy,
                        parentPermission: AppPermission.None,
                        tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.MultiTenancy_Read, 
                    new PermissionModel(
                        permissionName: "Multi-tenancy read",
                        permission: AppPermission.MultiTenancy_Read,
                        parentPermission: AppPermission.MultiTenancy,
                        tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.MultiTenancy_Write, 
                    new PermissionModel(
                        permissionName: "Multi-tenancy write",
                        permission: AppPermission.MultiTenancy_Write,
                        parentPermission: AppPermission.MultiTenancy,
                        tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.Users, 
                    new PermissionModel(
                        permissionName: "User management",
                        permission: AppPermission.Users,
                        parentPermission: AppPermission.None,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
                { 
                    AppPermission.Users_Read, 
                    new PermissionModel(
                        permissionName: "User read",
                        permission: AppPermission.Users_Read,
                        parentPermission: AppPermission.Users,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
                { 
                    AppPermission.Users_Write, 
                    new PermissionModel(
                        permissionName: "User write",
                        permission: AppPermission.Users_Write,
                        parentPermission: AppPermission.Users,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
                { 
                    AppPermission.Roles, 
                    new PermissionModel(
                        permissionName: "Role management",
                        permission: AppPermission.Roles,
                        parentPermission: AppPermission.None,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
                { 
                    AppPermission.Roles_Read, 
                    new PermissionModel(
                        permissionName: "Role read",
                        permission: AppPermission.Roles_Read,
                        parentPermission: AppPermission.Roles,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
                { 
                    AppPermission.Roles_Write, 
                    new PermissionModel(
                        permissionName: "Role write",
                        permission: AppPermission.Roles_Write,
                        parentPermission: AppPermission.Roles,
                        tenancySide: MultiTenancySide.Host | MultiTenancySide.Tenant)
                },
            };
            return permissions.ToImmutableDictionary();
        }
    }
}
