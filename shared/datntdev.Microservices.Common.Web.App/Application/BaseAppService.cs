using AutoMapper;
using datntdev.Microservices.Common.Application;
using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Web.App.Application
{
    public abstract class BaseAppService(IServiceProvider services) : IAppService 
    { 
        protected readonly IMapper _Mapper = services.GetRequiredService<IMapper>();
    }
}
