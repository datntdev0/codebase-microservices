using datntdev.Abp.MultiTenancy;
using datntdev.Microservice.Authorization.Users;

namespace datntdev.Microservice.MultiTenancy;

public class Tenant : AbpTenant<User>
{
    public Tenant()
    {
    }

    public Tenant(string tenancyName, string name)
        : base(tenancyName, name)
    {
    }
}
