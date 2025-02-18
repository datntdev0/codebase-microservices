using Abp.Application.Services;
using datntdev.Microservice.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
