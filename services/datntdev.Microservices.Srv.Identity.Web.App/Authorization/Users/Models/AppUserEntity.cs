using AutoMapper;
using datntdev.Microservices.Common;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Users.Models
{
    public class AppUserEntity : FullAuditEntity<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordPlainText { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Constants.Enum.AppPermission[] Permissions { get; set; } = [];
    }
}
