using datntdev.Abp.Web.Core.Mvc.Controllers;
using datntdev.Abp.IdentityFramework;
using datntdev.Abp.Runtime.Session;
using Microsoft.AspNetCore.Identity;
using datntdev.Microservice.Authorization.Users;
using datntdev.Microservice.MultiTenancy;
using System.Threading.Tasks;
using System;

namespace datntdev.Microservice.Controllers
{
    public abstract class MicroserviceControllerBase : AbpController
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected MicroserviceControllerBase()
        {
            LocalizationSourceName = MicroserviceConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

