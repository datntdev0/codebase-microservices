using datntdev.Abp.Domain.Entities;

namespace datntdev.Abp.Domain.Uow
{
    /// <summary>
    /// Standard filters of ABP.
    /// </summary>
    public static class AbpDataFilters
    {
        /// <summary>
        /// "SoftDelete".
        /// Soft delete filter.
        /// Prevents getting deleted data from database.
        /// See <see cref="ISoftDelete"/> interface.
        /// </summary>
        public const string SoftDelete = "SoftDelete";

        /// <summary>
        /// "MustHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MustHaveTenant = "MustHaveTenant";

        /// <summary>
        /// "MayHaveTenant".
        /// Tenant filter to prevent getting data that is
        /// not belong to current tenant.
        /// </summary>
        public const string MayHaveTenant = "MayHaveTenant";

        /// <summary>
        /// Standard parameters of ABP.
        /// </summary>
        public static class Parameters
        {
            /// <summary>
            /// "tenantId".
            /// </summary>
            public const string TenantId = "tenantId";

            /// <summary>
            /// "isDeleted".
            /// </summary>
            public const string IsDeleted = "isDeleted";
        }
    }

    /// <summary>
    /// Standard filters of ABP.
    /// </summary>
    public static class AbpAuditFields
    {
        public const string CreatorUserId = "CreatorUserId";

        public const string LastModifierUserId = "LastModifierUserId";

        public const string DeleterUserId = "DeleterUserId";

        public const string LastModificationTime = "LastModificationTime";

        public const string DeletionTime = "DeletionTime";
    }
}
