using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto
{
    public class RoleDto : BaseAuditDto<int>
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PermissionDto[] Permissions { get; set; } = [];
    }
}
