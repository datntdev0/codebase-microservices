using datntdev.Abp.Authorization;
using datntdev.Abp.Authorization.Roles;
using datntdev.Microservice.Authorization.Roles;
using AutoMapper;
using System.Linq;

namespace datntdev.Microservice.Authorization.Roles.Dto;

public class RoleMapProfile : Profile
{
    public RoleMapProfile()
    {
        // Role and permission
        CreateMap<Permission, string>().ConvertUsing(r => r.Name);
        CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

        CreateMap<CreateRoleInput, Role>();

        CreateMap<RoleDto, Role>();

        CreateMap<Role, RoleDto>()
            .ForMember(x => x.GrantedPermissions,
                opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));
    }
}
