using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantsAppService(IServiceProvider services) : BaseAppService, ITenantsAppService
    {
        private readonly TenantManager _manager = services.GetRequiredService<TenantManager>();

        public async Task<TenantDto> CreateAsync(TenantCreateDto request)
        {
            var tenantEntity = new Models.AppTenantEntity();
            tenantEntity = await _manager.CreateEntityAsync(tenantEntity);
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            await _manager.DeleteEntityAsync(id);
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<TenantDto> GetAsync(int id)
        {
            var tenantEntity = await _manager.GetEntityAsync(id);
            throw new NotImplementedException();
        }

        public async Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
        {
            var tenantEntity = new Models.AppTenantEntity();
            tenantEntity = await _manager.UpdateEntityAsync(tenantEntity);
            throw new NotImplementedException();
        }
    }
}
