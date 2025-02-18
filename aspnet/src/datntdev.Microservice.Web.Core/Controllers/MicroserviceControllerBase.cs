using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace datntdev.Microservice.Controllers
{
    public abstract class MicroserviceControllerBase : AbpController
    {
        protected MicroserviceControllerBase()
        {
            LocalizationSourceName = MicroserviceConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
