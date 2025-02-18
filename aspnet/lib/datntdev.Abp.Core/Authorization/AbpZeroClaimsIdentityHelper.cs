using System;
using System.Security.Claims;
using datntdev.Abp.Runtime.Security;

namespace datntdev.Abp.Authorization;

public static class AbpZeroClaimsIdentityHelper
{
    public static int? GetTenantId(ClaimsPrincipal principal)
    {
        var tenantIdOrNull = principal?.FindFirstValue(AbpClaimTypes.TenantId);
        if (tenantIdOrNull == null)
        {
            return null;
        }

        return Convert.ToInt32(tenantIdOrNull);
    }
}