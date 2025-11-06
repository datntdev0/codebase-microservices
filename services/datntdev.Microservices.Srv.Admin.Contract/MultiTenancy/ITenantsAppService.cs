using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;

namespace datntdev.Microservices.Srv.Admin.Contract.MultiTenancy
{
    public interface ITenantsAppService : IAppService<int, TenantDto, TenantCreateDto, TenantUpdateDto>
    {
        Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request);
    }
}
