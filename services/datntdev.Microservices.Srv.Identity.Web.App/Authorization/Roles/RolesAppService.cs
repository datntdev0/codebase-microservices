using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles
{
    internal class RolesAppService(IServiceProvider services) : BaseAppService(services), IRolesAppService
    {
        private readonly RoleManager _manager = services.GetRequiredService<RoleManager>();
        private readonly RoleCreateValidator _createValidator = services.GetRequiredService<RoleCreateValidator>();
        private readonly RoleUpdateValidator _updateValidator = services.GetRequiredService<RoleUpdateValidator>();
        private readonly UserManager _userManager = services.GetRequiredService<UserManager>();

        public async Task<RoleDto> CreateAsync(RoleCreateDto request)
        {
            _createValidator.ValidateAndThrow(request);

            var roleEntity = _Mapper.Map<AppRoleEntity>(request);
            
            // Load users based on provided user IDs
            roleEntity.Users = _userManager.GetQueryable()
                .Where(u => request.UserIds.Contains(u.Id))
                .ToList();

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

        [Route("{id:required:int}/permissions")]
        public async Task<PaginatedResult<PermissionDto>> GetAllPermissionsAsync(int id, PaginatedRequest request)
        {
            var roleEntity = await _manager.GetEntityAsync(id);
            var allPermissions = _Mapper.Map<IEnumerable<PermissionDto>>(roleEntity.Permissions);
            return new PaginatedResult<PermissionDto>(allPermissions, request.Limit, request.Offset);
        }

        [Route("{id:required:int}/users")]
        public async Task<PaginatedResult<UserListDto>> GetAllUsersAsync(int id, PaginatedRequest request)
        {
            var roleEntityTask = await _manager.GetEntityAsync(id);
            var allUsers = _Mapper.Map<IEnumerable<UserListDto>>(roleEntityTask.Users);
            return new PaginatedResult<UserListDto>(allUsers, request.Limit, request.Offset);
        }

        public async Task<RoleDto> GetAsync(int id)
        {
            var roleEntity = await _manager.GetEntityAsync(id);
            return _Mapper.Map<RoleDto>(roleEntity);
        }

        public async Task<RoleDto> UpdateAsync(int id, RoleUpdateDto request)
        {
            _updateValidator.ValidateAndThrow((id, request));

            var roleEntity = await _manager.GetEntityAsync(id);
    
            // Map basic properties
            roleEntity = _Mapper.Map(request, roleEntity);
    
            // Update permissions: start with existing, add new ones, remove specified ones
            var currentPermissions = roleEntity.Permissions.ToHashSet();
            foreach (var permission in request.AppendPermissions)
            {
                currentPermissions.Add(permission);
            }
            foreach (var permission in request.RemovePermissions)
            {
                currentPermissions.Remove(permission);
            }

            roleEntity.Permissions = currentPermissions.ToArray();
    
            // Update users: start with existing, add new ones, remove specified ones
            var currentUserIds = roleEntity.Users.Select(u => u.Id).ToHashSet();
            foreach (var userId in request.AppendUserIds)
            {
                currentUserIds.Add(userId);
            }
            foreach (var userId in request.RemoveUserIds)
            {
                currentUserIds.Remove(userId);
            }
    
            // Load the users based on final user ID list
            roleEntity.Users = _userManager.GetQueryable()
                .Where(u => currentUserIds.Contains(u.Id))
                .ToList();
    
            roleEntity = await _manager.UpdateEntityAsync(roleEntity);
            return _Mapper.Map<RoleDto>(roleEntity);
        }
    }
}
