using datntdev.Microservices.Common;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto
{
    public class PermissionDto
    {
        public Constants.Enum.AppPermission Permission { get; set; }
        public Constants.Enum.AppPermission Parent { get; set; }
        public bool IsGranted { get; set; }
    }
}
