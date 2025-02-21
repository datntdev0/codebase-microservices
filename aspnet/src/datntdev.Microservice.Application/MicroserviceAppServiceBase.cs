using datntdev.Abp.Application.Services;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.MultiTenancy;

namespace datntdev.Microservice;

/// <summary>
/// Derive your application services from this class.
/// </summary>
public abstract class MicroserviceAppServiceBase : ApplicationService
{
    public TenantManager TenantManager { get; set; }

    public UserManager UserManager { get; set; }

    protected MicroserviceAppServiceBase()
    {
        LocalizationSourceName = MicroserviceConsts.LocalizationSourceName;
    }
}
