using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles
{
    public interface IRolesAppService : IAppService<int, RoleDto, RoleCreateDto, RoleUpdateDto>
    {
        Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request);
        Task<PaginatedResult<UserListDto>> GetAllUsersAsync(int id, PaginatedRequest request);
        Task<PaginatedResult<PermissionDto>> GetAllPermissionsAsync(int id, PaginatedRequest request);
    }
}
