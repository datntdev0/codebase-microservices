using static datntdev.Microservices.Common.Constants.Enum;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Roles.Dto
{
    public class RoleUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AppPermission[] AppendPermissions { get; set; } = [];
        public AppPermission[] RemovePermissions { get; set; } = [];
        public long[] AppendUserIds { get; set; } = [];
        public long[] RemoveUserIds { get; set; } = [];
    }
}
