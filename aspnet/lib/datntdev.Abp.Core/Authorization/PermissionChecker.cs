using System.Threading.Tasks;
using datntdev.Abp.Authorization.Roles;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Runtime.Session;
using Castle.Core.Logging;

namespace datntdev.Abp.Authorization;

/// <summary>
/// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
/// </summary>
/// <typeparam name="TRole"></typeparam>
/// <typeparam name="TUser"></typeparam>
public class PermissionChecker<TRole, TUser> : IPermissionChecker, ITransientDependency, IIocManagerAccessor
    where TRole : AbpRole<TUser>, new()
    where TUser : AbpUser<TUser>
{
    private readonly AbpUserManager<TRole, TUser> _userManager;

    public IIocManager IocManager { get; set; }

    public ILogger Logger { get; set; }

    public IAbpSession AbpSession { get; set; }

    public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }

    public IUnitOfWorkManager UnitOfWorkManager { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PermissionChecker(AbpUserManager<TRole, TUser> userManager)
    {
        _userManager = userManager;

        Logger = NullLogger.Instance;
        AbpSession = NullAbpSession.Instance;
    }

    public virtual async Task<bool> IsGrantedAsync(string permissionName)
    {
        return AbpSession.UserId.HasValue && await IsGrantedAsync(AbpSession.UserId.Value, permissionName);
    }

    public virtual bool IsGranted(string permissionName)
    {
        return AbpSession.UserId.HasValue && IsGranted(AbpSession.UserId.Value, permissionName);
    }

    public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
    {
        return await _userManager.IsGrantedAsync(userId, permissionName);
    }

    public virtual bool IsGranted(long userId, string permissionName)
    {
        return _userManager.IsGranted(userId, permissionName);
    }

    public virtual async Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
    {
        return await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
        {
            if (CurrentUnitOfWorkProvider?.Current == null)
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return await IsGrantedAsync(user.UserId, permissionName);
            }
        });
    }

    public virtual bool IsGranted(UserIdentifier user, string permissionName)
    {
        return UnitOfWorkManager.WithUnitOfWork(() =>
        {
            if (CurrentUnitOfWorkProvider?.Current == null)
            {
                return IsGranted(user.UserId, permissionName);
            }

            using (CurrentUnitOfWorkProvider.Current.SetTenantId(user.TenantId))
            {
                return IsGranted(user.UserId, permissionName);
            }
        });
    }
}