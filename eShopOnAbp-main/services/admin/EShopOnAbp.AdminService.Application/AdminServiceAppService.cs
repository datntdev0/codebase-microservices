using System;
using System.Collections.Generic;
using System.Text;
using EShopOnAbp.AdminService.Localization;
using Volo.Abp.Application.Services;

namespace EShopOnAbp.AdminService
{
    /* Inherit your application services from this class.
     */
    public abstract class AdminServiceAppService : ApplicationService
    {
        protected AdminServiceAppService()
        {
            LocalizationResource = typeof(AdminServiceResource);
        }
    }
}
