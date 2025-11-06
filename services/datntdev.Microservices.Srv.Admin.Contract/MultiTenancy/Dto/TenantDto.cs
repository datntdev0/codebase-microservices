using datntdev.Microservices.Common.Models;

namespace datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto
{
    public class TenantDto : BaseAuditDto<int>
    {
        public string TenantName { get; set; } = default!;
    }
}
