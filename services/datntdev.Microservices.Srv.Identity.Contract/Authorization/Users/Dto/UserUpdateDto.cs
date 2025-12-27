using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto
{
    public class UserUpdateDto
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Password { get; set; }
        public AppPermission[] AppendPermissions { get; set; } = [];
        public AppPermission[] RemovePermissions { get; set; } = [];
        public int[] AppendRoleIds { get; set; } = [];
        public int[] RemoveRoleIds { get; set; } = [];
    }
}
