﻿using datntdev.Abp.Application.Services;
using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Authorization;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Extensions;
using datntdev.Abp.IdentityFramework;
using datntdev.Abp.Linq.Extensions;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Runtime.Security;
using datntdev.Microservice.Authorization.Permissions;
using datntdev.Microservice.Authorization.Roles;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.Editions;
using datntdev.Microservice.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace datntdev.Microservice.MultiTenancy;

[AbpAuthorize(PermissionNames.Pages_Tenants)]
public class TenantsAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultInput, CreateTenantInput, TenantDto>, ITenantsAppService
{
    private readonly TenantManager _tenantManager;
    private readonly EditionManager _editionManager;
    private readonly UserManager _userManager;
    private readonly RoleManager _roleManager;
    private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

    public TenantsAppService(
        IRepository<Tenant, int> repository,
        TenantManager tenantManager,
        EditionManager editionManager,
        UserManager userManager,
        RoleManager roleManager,
        IAbpZeroDbMigrator abpZeroDbMigrator)
        : base(repository)
    {
        _tenantManager = tenantManager;
        _editionManager = editionManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _abpZeroDbMigrator = abpZeroDbMigrator;
    }

    public override async Task<TenantDto> CreateAsync(CreateTenantInput input)
    {
        CheckCreatePermission();

        // Create tenant
        var tenant = ObjectMapper.Map<Tenant>(input);
        tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
            ? null
            : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

        var defaultEdition = await _editionManager.FindByNameAsync(EditionNames.DefaultEditionName);
        if (defaultEdition != null)
        {
            tenant.EditionId = defaultEdition.Id;
        }

        await _tenantManager.CreateAsync(tenant);
        await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

        // Create tenant database
        _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

        // We are working entities of new tenant, so changing tenant filter
        using (CurrentUnitOfWork.SetTenantId(tenant.Id))
        {
            // Create static roles for new tenant
            CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

            await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

            // Grant all permissions to admin role
            var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
            await _roleManager.GrantAllPermissionsAsync(adminRole);

            // Create admin user for the tenant
            var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
            await _userManager.InitializeOptionsAsync(tenant.Id);
            CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
            await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

            // Assign admin user to role!
            CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        return MapToEntityDto(tenant);
    }

    protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultInput input)
    {
        return Repository.GetAll()
            .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
            .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
    }

    protected override IQueryable<Tenant> ApplySorting(IQueryable<Tenant> query, PagedTenantResultInput input)
    {
        return query.OrderBy(input.Sorting);
    }

    protected override void MapToEntity(TenantDto updateInput, Tenant entity)
    {
        // Manually mapped since TenantDto contains non-editable properties too.
        entity.Name = updateInput.Name;
        entity.TenancyName = updateInput.TenancyName;
        entity.IsActive = updateInput.IsActive;
    }

    public override async Task DeleteAsync(int id)
    {
        CheckDeletePermission();

        var tenant = await _tenantManager.GetByIdAsync(id);
        await _tenantManager.DeleteAsync(tenant);
    }

    private void CheckErrors(IdentityResult identityResult)
    {
        identityResult.CheckErrors(LocalizationManager);
    }
}

