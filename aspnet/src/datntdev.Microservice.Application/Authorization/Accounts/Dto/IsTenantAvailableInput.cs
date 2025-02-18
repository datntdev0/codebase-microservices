using datntdev.Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.Authorization.Accounts.Dto;

public class IsTenantAvailableInput
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    public string TenancyName { get; set; }
}
