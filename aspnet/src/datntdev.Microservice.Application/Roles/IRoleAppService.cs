using Abp.Application.Services;
using Abp.Application.Services.Dto;
using datntdev.Microservice.Roles.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Roles;

public interface IRoleAppService : IAsyncCrudAppService<RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>
{
    Task<ListResultDto<PermissionDto>> GetAllPermissions();

    Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input);

    Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input);
}
