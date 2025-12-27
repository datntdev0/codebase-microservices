using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Users.Dto
{
    public class UserRoleListDto : BaseDto<int>, ICreated
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
