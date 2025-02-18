using datntdev.Abp.Authorization.Users;
using datntdev.Abp.AutoMapper;
using datntdev.Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.Microservice.MultiTenancy.Dto;

[AutoMapTo(typeof(Tenant))]
public class CreateTenantDto
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    [RegularExpression(AbpTenantBase.TenancyNameRegex)]
    public string TenancyName { get; set; }

    [Required]
    [StringLength(AbpTenantBase.MaxNameLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(AbpUserBase.MaxEmailAddressLength)]
    public string AdminEmailAddress { get; set; }

    [StringLength(AbpTenantBase.MaxConnectionStringLength)]
    public string ConnectionString { get; set; }

    public bool IsActive { get; set; }
}
