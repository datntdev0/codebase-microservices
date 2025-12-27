using AutoMapper;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUserRoleEntity, UserRoleListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role!.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Role!.Description));
            CreateMap<AppUserRoleEntity, RoleUserListDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User!.Username))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.User!.EmailAddress))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User!.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User!.LastName));
        }
    }
}