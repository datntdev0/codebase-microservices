using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models
{
    public class AppTenantEntity : BaseAuditEntity<int>
    {
        public string TenantName { get; set; } = default!;
    }
}
