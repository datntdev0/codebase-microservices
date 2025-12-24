using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto
{
    public class UserDto : BaseAuditDto<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public PermissionDto[] Permissions { get; set; } = [];
    }
}
