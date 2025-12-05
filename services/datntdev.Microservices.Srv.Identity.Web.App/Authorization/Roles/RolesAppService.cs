using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles
{
    internal class RolesAppService(IServiceProvider services) : BaseAppService(services), IRolesAppService
    {
        private readonly RoleManager _manager = services.GetRequiredService<RoleManager>();

        public async Task<RoleDto> CreateAsync(RoleCreateDto request)
        {
            var roleEntity = _Mapper.Map<AppRoleEntity>(request);
            roleEntity = await _manager.CreateEntityAsync(roleEntity);
            return _Mapper.Map<RoleDto>(roleEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _manager.DeleteEntityAsync(id);
        }

        public async Task<PaginatedResult<RoleListDto>> GetAllAsync(PaginatedRequest request)
        {
            var queryable = _manager.GetQueryable();
            var total = await queryable.CountAsync();
            var query = await queryable.Skip(request.Offset).Take(request.Limit).ToListAsync();
            var items = _Mapper.Map<List<RoleListDto>>(query);
            return new PaginatedResult<RoleListDto>
            {
                Items = items,
                Total = total,
                Limit = request.Limit,
                Offset = request.Offset,
            };
        }

        public async Task<RoleDto> GetAsync(int id)
        {
            var roleEntity = await _manager.GetEntityAsync(id);
            return _Mapper.Map<RoleDto>(roleEntity);
        }

        public async Task<RoleDto> UpdateAsync(int id, RoleUpdateDto request)
        {
            var roleEntity = await _manager.GetEntityAsync(id);
            roleEntity = _Mapper.Map(request, roleEntity);
            roleEntity = await _manager.UpdateEntityAsync(roleEntity);
            return _Mapper.Map<RoleDto>(roleEntity);
        }
    }
}
