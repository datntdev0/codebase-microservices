using datntdev.Abp.MultiTenancy;

namespace datntdev.Abp.Web.Models.AbpUserConfiguration
{
    public class AbpMultiTenancySidesConfigDto
    {
        public MultiTenancySides Host { get; private set; }

        public MultiTenancySides Tenant { get; private set; }

        public AbpMultiTenancySidesConfigDto()
        {
            Host = MultiTenancySides.Host;
            Tenant = MultiTenancySides.Tenant;
        }
    }
}