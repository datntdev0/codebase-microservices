using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantsAppService : BaseAppService, ITenantsAppService
    {
        public Task<TenantDto> CreateAsync(TenantCreateDto request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TenantDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
        {
            throw new NotImplementedException();
        }
    }
}
