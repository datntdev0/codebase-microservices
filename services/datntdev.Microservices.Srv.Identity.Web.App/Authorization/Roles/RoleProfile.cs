using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles
{
    internal class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleCreateDto, AppRoleEntity>();
            CreateMap<RoleUpdateDto, AppRoleEntity>();
            CreateMap<AppRoleEntity, RoleListDto>();
            CreateMap<AppRoleEntity, RoleDto>()
                .ForMember(x => x.Permissions, opt => opt.MapFrom<PermissionDtoResolver>());
        }
    }
}
