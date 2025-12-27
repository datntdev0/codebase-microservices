using datntdev.Microservices.Common.Models;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto
{
    public class RoleDto : BaseAuditDto<int>
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AppPermission[] Permissions { get; set; } = [];
    }
}
