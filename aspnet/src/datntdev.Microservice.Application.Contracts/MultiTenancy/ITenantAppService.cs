using datntdev.Abp.Application.Services;
using datntdev.Microservice.MultiTenancy.Dto;

namespace datntdev.Microservice.MultiTenancy;

public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
{
}

