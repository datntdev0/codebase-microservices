using AutoMapper;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models
{
    [AutoMap(typeof(TenantDto), ReverseMap = true)]
    [AutoMap(typeof(TenantListDto), ReverseMap = true)]
    [AutoMap(typeof(TenantCreateDto), ReverseMap = false)]
    [AutoMap(typeof(TenantUpdateDto), ReverseMap = false)]
    public class AppTenantEntity : BaseAuditEntity<int>
    {
        public string TenantName { get; set; } = default!;
    }
}
