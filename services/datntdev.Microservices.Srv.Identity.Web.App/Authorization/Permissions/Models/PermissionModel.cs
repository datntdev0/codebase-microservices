using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions.Models
{
    public class PermissionModel(
        AppPermission permission,
        AppPermission parentPermission = AppPermission.None,
        bool isTenantOnly = true)
    {
        public AppPermission Permission { get; set; } = permission;
        public AppPermission Parent { get; set; } = parentPermission;
        public bool IsTenantOnly { get; set; } = isTenantOnly;
    }
}
