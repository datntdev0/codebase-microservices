using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Zero.EntityFrameworkCore;

public interface IMultiTenantSeed
{
    AbpTenantBase Tenant { get; set; }
}