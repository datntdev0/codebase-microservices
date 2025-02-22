using datntdev.Abp.Application.Services;
using datntdev.Abp.Application.Services.Dto;
using datntdev.Microservice.Authorization.Roles.Dto;
using datntdev.Microservice.Authorization.Users.Dto;

namespace datntdev.Microservice.Authorization.Users;

public interface IUsersAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultInput, CreateUserInput, UserDto>
{
    Task ActivateAsync(EntityDto<long> user);
    Task DeactivateAsync(EntityDto<long> user);
    Task<ListResultDto<RoleDto>> GetRolesAsync();
}
