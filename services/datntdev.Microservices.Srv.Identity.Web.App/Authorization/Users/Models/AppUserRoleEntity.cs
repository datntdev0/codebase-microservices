using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models
{
    public class AppUserRoleEntity : ICreated
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }

        public AppUserEntity? User { get; set; }
        public AppRoleEntity? Role { get; set; }
    }
}
