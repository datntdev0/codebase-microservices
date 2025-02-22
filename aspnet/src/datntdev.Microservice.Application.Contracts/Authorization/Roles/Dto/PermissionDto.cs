using datntdev.Abp.Application.Services.Dto;
using datntdev.Abp.Authorization;
using datntdev.Abp.AutoMapper;

namespace datntdev.Microservice.Authorization.Roles.Dto;

[AutoMapFrom(typeof(Permission))]
public class PermissionDto : EntityDto<long>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }
}
