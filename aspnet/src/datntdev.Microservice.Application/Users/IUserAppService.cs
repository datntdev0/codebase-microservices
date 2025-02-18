using datntdev.Abp.Application.Services;
using datntdev.Abp.Application.Services.Dto;
using datntdev.Microservice.Roles.Dto;
using datntdev.Microservice.Users.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Users;

public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
{
    Task DeActivate(EntityDto<long> user);
    Task Activate(EntityDto<long> user);
    Task<ListResultDto<RoleDto>> GetRoles();
    Task ChangeLanguage(ChangeUserLanguageDto input);

    Task<bool> ChangePassword(ChangePasswordDto input);
}
