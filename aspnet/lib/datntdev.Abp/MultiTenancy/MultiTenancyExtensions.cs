using datntdev.Abp.Domain.Entities;

namespace datntdev.Abp.MultiTenancy
{
    /// <summary>
    /// Extension methods for multi-tenancy.
    /// </summary>
    public static class MultiTenancyExtensions
    {
        /// <summary>
        /// Gets multi-tenancy side (<see cref="MultiTenancySides"/>) of an object that implements <see cref="IMayHaveTenant"/>.
        /// </summary>
        /// <param name="obj">The object</param>
        public static MultiTenancySides GetMultiTenancySide(this IMayHaveTenant obj)
        {
            return obj.TenantId.HasValue
                ? MultiTenancySides.Tenant
                : MultiTenancySides.Host;
        }
    }
}