using datntdev.Microservices.Common.Web.App.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Srv.Identity.Web.App.Authorization.Permissions
{
    internal class PermissionsAppService(IServiceProvider services)
        : BaseAppService(services), IPermissionsAppService
    {
        private readonly PermissionProvider _permissionProvider = services.GetRequiredService<PermissionProvider>();

        public Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            var fullPermissions = _permissionProvider.GetAllPermissions();
            var permissionDtos = _Mapper.Map<IEnumerable<PermissionDto>>(fullPermissions);
            return Task.FromResult(permissionDtos);
        }
    }
}
