using System.Threading.Tasks;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Extensions;
using datntdev.Abp.MultiTenancy;
using datntdev.Abp.Runtime.Session;

namespace datntdev.Abp.Zero.EntityFrameworkCore;

/// <summary>
/// Implements <see cref="IDbPerTenantConnectionStringResolver"/> to dynamically resolve
/// connection string for a multi tenant application.
/// </summary>
public class DbPerTenantConnectionStringResolver : DefaultConnectionStringResolver, IDbPerTenantConnectionStringResolver
{
    /// <summary>
    /// Reference to the session.
    /// </summary>
    public IAbpSession AbpSession { get; set; }

    private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
    private readonly ITenantCache _tenantCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbPerTenantConnectionStringResolver"/> class.
    /// </summary>
    public DbPerTenantConnectionStringResolver(
        IAbpStartupConfiguration configuration,
        ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
        ITenantCache tenantCache)
        : base(configuration)
    {
        _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        _tenantCache = tenantCache;

        AbpSession = NullAbpSession.Instance;
    }

    public override string GetNameOrConnectionString(ConnectionStringResolveArgs args)
    {
        if (args.MultiTenancySide == MultiTenancySides.Host)
        {
            return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(null, args));
        }

        return GetNameOrConnectionString(new DbPerTenantConnectionStringResolveArgs(GetCurrentTenantId(), args));
    }

    public virtual string GetNameOrConnectionString(DbPerTenantConnectionStringResolveArgs args)
    {
        if (args.TenantId == null)
        {
            //Requested for host
            return base.GetNameOrConnectionString(args);
        }

        var tenantCacheItem = _tenantCache.Get(args.TenantId.Value);
        if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
        {
            //Tenant has not dedicated database
            return base.GetNameOrConnectionString(args);
        }

        return tenantCacheItem.ConnectionString;
    }


    public override async Task<string> GetNameOrConnectionStringAsync(ConnectionStringResolveArgs args)
    {
        if (args.MultiTenancySide == MultiTenancySides.Host)
        {
            return await GetNameOrConnectionStringAsync(new DbPerTenantConnectionStringResolveArgs(null, args));
        }

        return await GetNameOrConnectionStringAsync(new DbPerTenantConnectionStringResolveArgs(GetCurrentTenantId(), args));
    }

    public virtual async Task<string> GetNameOrConnectionStringAsync(DbPerTenantConnectionStringResolveArgs args)
    {
        if (args.TenantId == null)
        {
            //Requested for host
            return await base.GetNameOrConnectionStringAsync(args);
        }

        var tenantCacheItem = await _tenantCache.GetAsync(args.TenantId.Value);
        if (tenantCacheItem.ConnectionString.IsNullOrEmpty())
        {
            //Tenant has not dedicated database
            return await base.GetNameOrConnectionStringAsync(args);
        }

        return tenantCacheItem.ConnectionString;
    }

    protected virtual int? GetCurrentTenantId()
    {
        return _currentUnitOfWorkProvider.Current != null
            ? _currentUnitOfWorkProvider.Current.GetTenantId()
            : AbpSession.TenantId;
    }
}