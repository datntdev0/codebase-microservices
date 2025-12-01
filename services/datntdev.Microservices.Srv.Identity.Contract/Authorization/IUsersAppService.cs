using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization
{
    public interface IUsersAppService : IAppService
    {
        Task<UserDto> GetAsync(long id);
        Task<UserDto> CreateAsync(UserCreateDto request);
        Task<UserDto> UpdateAsync(long id, UserUpdateDto request);
        Task DeleteAsync(long id);
        Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request);
    }
}
