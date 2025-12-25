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
                    new PermissionModel(AppPermission.MultiTenancy, tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.MultiTenancy_Read, 
                    new PermissionModel(AppPermission.MultiTenancy_Read, AppPermission.MultiTenancy, tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.MultiTenancy_Write, 
                    new PermissionModel(AppPermission.MultiTenancy_Write, AppPermission.MultiTenancy, tenancySide: MultiTenancySide.Host)
                },
                { 
                    AppPermission.Users, 
                    new PermissionModel(AppPermission.Users)
                },
                { 
                    AppPermission.Users_Read, 
                    new PermissionModel(AppPermission.Users_Read, AppPermission.Users)
                },
                { 
                    AppPermission.Users_Write, 
                    new PermissionModel(AppPermission.Users_Write, AppPermission.Users)
                },
                { 
                    AppPermission.Roles, 
                    new PermissionModel(AppPermission.Roles)
                },
                { 
                    AppPermission.Roles_Read, 
                    new PermissionModel(AppPermission.Roles_Read, AppPermission.Roles)
                },
                { 
                    AppPermission.Roles_Write, 
                    new PermissionModel(AppPermission.Roles_Write, AppPermission.Roles)
                },
            };
            return permissions.ToImmutableDictionary();
        }
    }
}
