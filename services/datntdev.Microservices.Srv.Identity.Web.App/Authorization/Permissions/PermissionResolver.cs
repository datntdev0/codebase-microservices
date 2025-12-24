using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions
{
    internal class PermissionDtoResolver(PermissionProvider permissionProvider) :
        IValueResolver<AppRoleEntity, RoleDto, PermissionDto[]>,
        IValueResolver<AppUserEntity, UserDto, PermissionDto[]>
    {
        private readonly PermissionProvider _permissionProvider = permissionProvider;

        public PermissionDto[] Resolve(AppRoleEntity source, RoleDto destination, PermissionDto[] destMember, ResolutionContext context)
        {
            return MapPermissionDtos(source.Permissions);
        }

        public PermissionDto[] Resolve(AppUserEntity source, UserDto destination, PermissionDto[] destMember, ResolutionContext context)
        {
            return MapPermissionDtos(source.Permissions);
        }

        private PermissionDto[] MapPermissionDtos(AppPermission[] grantedPermissions)
        {
            return _permissionProvider.GetAllPermissions()
                .Select(p => new PermissionDto
                {
                    Parent = p.Parent,
                    Permission = p.Permission,
                    IsGranted = grantedPermissions.Contains(p.Permission),
                })
                .ToArray();
        }
    }
}
