using AutoMapper;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;
using datntdev.Microservices.Srv.Identity.Web.App.Authorization.Roles.Models;
using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models
{
    [AutoMap(typeof(UserDto), ReverseMap = true)]
    [AutoMap(typeof(UserListDto), ReverseMap = true)]
    [AutoMap(typeof(UserCreateDto), ReverseMap = false)]
    [AutoMap(typeof(UserUpdateDto), ReverseMap = false)]
    public class AppUserEntity : FullAuditEntity<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordPlainText { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public AppPermission[] Permissions { get; set; } = [];

        public List<AppRoleEntity> Roles { get; set; } = [];
    }
}
