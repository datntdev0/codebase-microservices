using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users
{
    internal class UsersAppService(IServiceProvider services) : BaseAppService(services), IUsersAppService
    {
        private readonly UserManager _userManager = services.GetRequiredService<UserManager>();
        private readonly RoleManager _roleManager = services.GetRequiredService<RoleManager>();
        private readonly UserCreateValidator _createValidator = services.GetRequiredService<UserCreateValidator>();
        private readonly UserUpdateValidator _updateValidator = services.GetRequiredService<UserUpdateValidator>();

        public async Task<UserDto> CreateAsync(UserCreateDto request)
        {
            _createValidator.ValidateAndThrow(request);
            
            var userEntity = _Mapper.Map<AppUserEntity>(request);
            userEntity.PasswordPlainText = request.Password;
            
            // Load roles based on provided role IDs
            userEntity.Roles = _roleManager.GetQueryable()
                .Where(r => request.RoleIds.Contains(r.Id))
                .ToList();

            userEntity = await _userManager.CreateEntityAsync(userEntity);
            return _Mapper.Map<UserDto>(userEntity);
        }

        public async Task DeleteAsync(long id)
        {
            await _userManager.DeleteEntityAsync(id);
        }

        public async Task<PaginatedResult<UserListDto>> GetAllAsync(PaginatedRequest request)
        {
            var queryable = _userManager.GetQueryable();
            var total = await queryable.CountAsync();
            var query = await queryable.OrderBy(x => x.CreatedAt)
                .Skip(request.Offset).Take(request.Limit).ToListAsync();
            var items = _Mapper.Map<List<UserListDto>>(query);
            return new PaginatedResult<UserListDto>
            {
                Items = items,
                Total = total,
                Limit = request.Limit,
                Offset = request.Offset,
            };
        }

        [Route("{id:required:long}/roles")]
        public async Task<PaginatedResult<UserRoleListDto>> GetAllRolesAsync(long id, PaginatedRequest request)
        {
            await _userManager.GetEntityAsync(id); // Ensure user exists

            var queryable = _userManager.GetUserRoleByUserId(id);
            var total = await queryable.CountAsync();
            var query = await queryable.OrderBy(x => x.CreatedAt)
                .Skip(request.Offset).Take(request.Limit).ToListAsync();
            var items = _Mapper.Map<List<UserRoleListDto>>(query);
            return new PaginatedResult<UserRoleListDto>
            {
                Items = items,
                Total = total,
                Limit = request.Limit,
                Offset = request.Offset,
            };
        }

        public async Task<UserDto> GetAsync(long id)
        {
            var userEntity = await _userManager.GetEntityAsync(id);
            return _Mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto> UpdateAsync(long id, UserUpdateDto request)
        {
            _updateValidator.ValidateAndThrow((id, request));

            var userEntity = await _userManager.GetEntityAsync(id);

            // Map basic properties
            userEntity = _Mapper.Map(request, userEntity);
            userEntity.PasswordPlainText = request.Password!;

            // Update permissions: start with existing, add new ones, remove specified ones
            var currentPermissions = userEntity.Permissions.ToHashSet();
            foreach (var permission in request.AppendPermissions)
            {
                currentPermissions.Add(permission);
            }
            foreach (var permission in request.RemovePermissions)
            {
                currentPermissions.Remove(permission);
            }

            userEntity.Permissions = currentPermissions.ToArray();

            // Update roles: start with existing, add new ones, remove specified ones
            var currentRoleIds = userEntity.Roles.Select(r => r.Id).ToHashSet();
            foreach (var roleId in request.AppendRoleIds)
            {
                currentRoleIds.Add(roleId);
            }
            foreach (var roleId in request.RemoveRoleIds)
            {
                currentRoleIds.Remove(roleId);
            }

            // Load the roles based on final role ID list
            userEntity.Roles = _roleManager.GetQueryable()
                .Where(r => currentRoleIds.Contains(r.Id))
                .ToList();

            userEntity = await _userManager.UpdateEntityAsync(userEntity);
            return _Mapper.Map<UserDto>(userEntity);
        }
    }
}
