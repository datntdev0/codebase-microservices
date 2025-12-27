using datntdev.Microservices.Common.Application;
using datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions.Dto;

namespace datntdev.Microservices.Srv.Identity.Contract.Authorization.Permissions
{
    public interface IPermissionsAppService : IAppService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
    }
}
