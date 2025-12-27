using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles
{
    public interface IRolesAppService : IAppService<int, RoleDto, RoleCreateDto, RoleUpdateDto>
    {
        Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request);
        Task<PaginatedResult<RoleUserListDto>> GetAllUsersAsync(int id, PaginatedRequest request);
    }
}
