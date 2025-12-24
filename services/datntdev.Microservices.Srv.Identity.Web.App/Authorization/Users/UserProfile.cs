using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserCreateDto, AppUserEntity>();
            CreateMap<UserUpdateDto, AppUserEntity>();
            CreateMap<AppUserEntity, UserListDto>();
            CreateMap<AppUserEntity, UserDto>()
                .ForMember(x => x.Permissions, opt => opt.MapFrom<PermissionDtoResolver>());
        }
    }
}
