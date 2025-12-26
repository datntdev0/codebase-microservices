using datntdev.Microservices.Common.Models;
using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy;
using datntdev.Microservices.Srv.Admin.Contract.MultiTenancy.Dto;
using datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Admin.Web.App.MultiTenancy
{
    internal class TenantsAppService(IServiceProvider services) : BaseAppService(services), ITenantsAppService
    {
        private readonly TenantManager _manager = services.GetRequiredService<TenantManager>();
        private readonly TenantCreateValidator _createValidator = services.GetRequiredService<TenantCreateValidator>();
        private readonly TenantUpdateValidator _updateValidator = services.GetRequiredService<TenantUpdateValidator>();

        public async Task<TenantDto> CreateAsync(TenantCreateDto request)
        {
            _createValidator.ValidateAndThrow(request);

            var tenantEntity = _Mapper.Map<AppTenantEntity>(request);
            tenantEntity = await _manager.CreateEntityAsync(tenantEntity);
            return _Mapper.Map<TenantDto>(tenantEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _manager.DeleteEntityAsync(id);
        }

        public async Task<PaginatedResult<TenantListDto>> GetAllAsync(PaginatedRequest request)
        {
            var queryable = _manager.GetQueryable();
            var total = await queryable.CountAsync();
            var query = await queryable.Skip(request.Offset).Take(request.Limit).ToListAsync();
            var items = _Mapper.Map<List<TenantListDto>>(query);
            return new PaginatedResult<TenantListDto>
            {
                Items = items,
                Total = total,
                Limit = request.Limit,
                Offset = request.Offset,
            };
        }

        public async Task<TenantDto> GetAsync(int id)
        {
            var tenantEntity = await _manager.GetEntityAsync(id);
            return _Mapper.Map<TenantDto>(tenantEntity);
        }

        public async Task<TenantDto> UpdateAsync(int id, TenantUpdateDto request)
        {
            _updateValidator.ValidateAndThrow((id, request));

            var tenantEntity = await _manager.GetEntityAsync(id);
            tenantEntity = _Mapper.Map(request, tenantEntity);
            tenantEntity = await _manager.UpdateEntityAsync(tenantEntity);
            return _Mapper.Map<TenantDto>(tenantEntity);
        }
    }
}
