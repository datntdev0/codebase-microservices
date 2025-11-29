using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto
{
    public class TenantListDto : BaseDto<int>
    {
        public string TenantName { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
