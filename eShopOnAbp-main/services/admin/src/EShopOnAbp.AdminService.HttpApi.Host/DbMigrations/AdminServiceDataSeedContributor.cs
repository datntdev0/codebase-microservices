using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace EShopOnAbp.AdminService.DbMigrations;

public class AdminServiceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IPermissionManager _permissionManager;
    protected const string IdentityUsersDefaultPermission = "AbpIdentity.Users";
    protected const string IdentityUsersLookupPermission = "AbpIdentity.UserLookup";

    public AdminServiceDataSeedContributor(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await SeedClientCredentialPermissionsAsync();
    }

    private async Task SeedClientCredentialPermissionsAsync()
    {
        // await _permissionManager.SetAsync(IdentityUsersDefaultPermission, ClientPermissionValueProvider.ProviderName, "EShopOnAbp_AdminService", true);
        // await _permissionManager.SetAsync(IdentityUsersDefaultPermission, ClientPermissionValueProvider.ProviderName, "EShopOnAbp_CmskitService", true);
        // await _permissionManager.SetAsync(IdentityUsersLookupPermission, ClientPermissionValueProvider.ProviderName, "EShopOnAbp_CmskitService", true);
    }
}