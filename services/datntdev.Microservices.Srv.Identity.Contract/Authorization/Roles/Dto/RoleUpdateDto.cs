using datntdev.Microservices.Common;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto
{
    public class RoleUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Constants.Enum.AppPermission[] Permissions { get; set; } = [];
    }
}
