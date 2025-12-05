using datntdev.Microservices.Common;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto
{
    public class RoleCreateDto
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Constants.Enum.AppPermission[] Permissions { get; set; } = [];
    }
}
