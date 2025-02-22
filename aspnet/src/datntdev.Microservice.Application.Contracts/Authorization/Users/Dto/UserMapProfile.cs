using datntdev.Microservice.Authorization.Users;
using AutoMapper;

namespace datntdev.Microservice.Authorization.Users.Dto;

public class UserMapProfile : Profile
{
    public UserMapProfile()
    {
        CreateMap<UserDto, User>();
        CreateMap<UserDto, User>()
            .ForMember(x => x.Roles, opt => opt.Ignore())
            .ForMember(x => x.CreationTime, opt => opt.Ignore());

        CreateMap<CreateUserInput, User>();
        CreateMap<CreateUserInput, User>().ForMember(x => x.Roles, opt => opt.Ignore());
    }
}
