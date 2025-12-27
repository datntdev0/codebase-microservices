using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto
{
    public class PermissionDto
    {
        public string PermissionName { get; set; } = string.Empty;
        public AppPermission Permission { get; set; }
        public AppPermission Parent { get; set; }
    }
}
