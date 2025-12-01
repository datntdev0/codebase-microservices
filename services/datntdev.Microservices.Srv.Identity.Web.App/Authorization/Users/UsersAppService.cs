using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    internal class UsersAppService(IServiceProvider services) : BaseAppService(services), IUsersAppService
    {
        private readonly UserManager _manager = services.GetRequiredService<UserManager>();

        public async Task<UserDto> CreateAsync(UserCreateDto request)
        {
            var userEntity = _Mapper.Map<AppUserEntity>(request);
            userEntity.PasswordPlainText = request.Password;
            userEntity = await _manager.CreateEntityAsync(userEntity);
            return _Mapper.Map<UserDto>(userEntity);
        }

        public async Task DeleteAsync(long id)
        {
            await _manager.DeleteEntityAsync(id);
        }

        public async Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request)
        {
            var queryable = _manager.GetQueryable();
            var total = await queryable.CountAsync();
            var query = await queryable.Skip(request.Offset).Take(request.Limit).ToListAsync();
            var items = _Mapper.Map<List<UserListDto>>(query);
            return new PaginatedResult<UserListDto>
            {
                Items = items,
                Total = total,
                Limit = request.Limit,
                Offset = request.Offset,
            };
        }

        public async Task<UserDto> GetAsync(long id)
        {
            var userEntity = await _manager.GetEntityAsync(id);
            return _Mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> UpdateAsync(long id, UserUpdateDto request)
        {
            var userEntity = await _manager.GetEntityAsync(id);
            userEntity = _Mapper.Map(request, userEntity);
            if (!string.IsNullOrEmpty(request.Password))
            {
                userEntity.PasswordPlainText = request.Password;
            }
            userEntity = await _manager.UpdateEntityAsync(userEntity);
            return _Mapper.Map<UserDto>(userEntity);
        }
    }
}
