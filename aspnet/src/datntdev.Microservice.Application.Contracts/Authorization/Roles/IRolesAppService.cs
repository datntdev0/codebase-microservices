using datntdev.Abp.Application.Services;
using datntdev.Abp.Application.Services.Dto;
using datntdev.Microservice.Authorization.Roles.Dto;

namespace datntdev.Microservice.Authorization.Roles;

public interface IRolesAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultInput, CreateRoleInput, RoleDto>
{
    Task<ListResultDto<PermissionDto>> GetPermissionsAsync();
}
