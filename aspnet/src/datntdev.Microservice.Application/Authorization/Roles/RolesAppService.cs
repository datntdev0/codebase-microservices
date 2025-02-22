using datntdev.Abp.Application.Services;
using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Authorization;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Extensions;
using datntdev.Abp.IdentityFramework;
using datntdev.Abp.Linq.Extensions;
using datntdev.Microservice.Authorization.Permissions;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace datntdev.Microservice.Authorization.Roles;

[AbpAuthorize(PermissionNames.Pages_Roles)]
public class RolesAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultInput, CreateRoleInput, RoleDto>, IRolesAppService
{
    private readonly RoleManager _roleManager;
    private readonly UserManager _userManager;

    public RolesAppService(IRepository<Role> repository, RoleManager roleManager, UserManager userManager)
        : base(repository)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public override async Task<RoleDto> CreateAsync(CreateRoleInput input)
    {
        CheckCreatePermission();

        var role = ObjectMapper.Map<Role>(input);
        role.SetNormalizedName();

        CheckErrors(await _roleManager.CreateAsync(role));

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    public override async Task<RoleDto> UpdateAsync(RoleDto input)
    {
        CheckUpdatePermission();

        var role = await _roleManager.GetRoleByIdAsync(input.Id);

        ObjectMapper.Map(input, role);

        CheckErrors(await _roleManager.UpdateAsync(role));

        var grantedPermissions = PermissionManager
            .GetAllPermissions()
            .Where(p => input.GrantedPermissions.Contains(p.Name))
            .ToList();

        await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

        return MapToEntityDto(role);
    }

    public override async Task DeleteAsync(int id)
    {
        CheckDeletePermission();

        var role = await _roleManager.FindByIdAsync(id.ToString());
        var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

        foreach (var user in users)
        {
            CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
        }

        CheckErrors(await _roleManager.DeleteAsync(role));
    }

    public Task<ListResultDto<PermissionDto>> GetPermissionsAsync()
    {
        var permissions = PermissionManager.GetAllPermissions();

        return Task.FromResult(new ListResultDto<PermissionDto>(
            ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
        ));
    }

    protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultInput input)
    {
        return Repository.GetAllIncluding(x => x.Permissions)
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
            || x.DisplayName.Contains(input.Keyword)
            || x.Description.Contains(input.Keyword));
    }

    protected override async Task<Role> GetEntityByIdAsync(int id)
    {
        return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
    }

    protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultInput input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected virtual void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}

