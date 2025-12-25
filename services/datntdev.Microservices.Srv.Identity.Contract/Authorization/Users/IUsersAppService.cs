using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Users
{
    public interface IUsersAppService : IAppService<long, UserDto, UserCreateDto, UserUpdateDto>
    {
        Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request);
        Task<PaginatedResult<RoleListDto>> GetAllRolesAsync(long id, PaginatedRequest request);
        Task<PaginatedResult<PermissionDto>> GetAllPermissionsAsync(long id, PaginatedRequest request);
    }
}
