using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models
{
    public class AppRoleEntity : FullAuditEntity<int>, ITenancy
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Constants.Enum.AppPermission[] Permissions { get; set; } = [];
    }
}
