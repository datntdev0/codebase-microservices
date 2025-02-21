using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.AutoMapper;
using datntdev.Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.MultiTenancy.Dto;

[AutoMapFrom(typeof(Tenant))]
public class TenantDto : EntityDto
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    [RegularExpression(AbpTenantBase.TenancyNameRegex)]
    public string TenancyName { get; set; }

    [Required]
    [StringLength(AbpTenantBase.MaxNameLength)]
    public string Name { get; set; }

    public bool IsActive { get; set; }
}
