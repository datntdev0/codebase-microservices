using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Dto
{
    public class UserDto : BaseAuditDto<long>
    {
        public string Username { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
