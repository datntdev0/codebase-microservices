using AutoMapper;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models
{
    [AutoMap(typeof(RoleDto), ReverseMap = true)]
    [AutoMap(typeof(RoleListDto), ReverseMap = true)]
    [AutoMap(typeof(RoleCreateDto), ReverseMap = false)]
    [AutoMap(typeof(RoleUpdateDto), ReverseMap = false)]
    public class AppRoleEntity : FullAuditEntity<int>, ITenancy
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AppPermission[] Permissions { get; set; } = [];

        public List<AppUserEntity> Users { get; set; } = [];
    }
}
