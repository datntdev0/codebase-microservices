using datntdev.Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Models.Auth;

public class IsTenantAvailableInput
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    public string TenancyName { get; set; }
}
