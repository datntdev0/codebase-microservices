using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using datntdev.Microservice.MultiTenancy;

namespace datntdev.Microservice.Sessions.Dto;

[AutoMapFrom(typeof(Tenant))]
public class TenantLoginInfoDto : EntityDto
{
    public string TenancyName { get; set; }

    public string Name { get; set; }
}
