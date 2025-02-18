using System.Security.Claims;

namespace datntdev.Abp.Runtime.Session
{
    public interface IPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }
    }
}
