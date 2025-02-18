using Abp.Application.Services;
using datntdev.Microservice.Sessions.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
