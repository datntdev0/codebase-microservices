using datntdev.Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Models.Auth;

public class GetTenantStatusInput
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    public string TenancyName { get; set; }
}
