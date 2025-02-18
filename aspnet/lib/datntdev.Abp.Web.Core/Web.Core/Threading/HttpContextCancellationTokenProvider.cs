using datntdev.Abp.Dependency;
using datntdev.Abp.Threading;
using Microsoft.AspNetCore.Http;
using System.Threading;
using datntdev.Abp.Runtime;

namespace datntdev.Abp.Web.Core.Threading;

public class HttpContextCancellationTokenProvider : CancellationTokenProviderBase, ITransientDependency
{
    public override CancellationToken Token
    {
        get
        {
            if (OverridedValue != null)
            {
                return OverridedValue.CancellationToken;
            }
            return _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCancellationTokenProvider(
        IHttpContextAccessor httpContextAccessor,
        IAmbientScopeProvider<CancellationTokenOverride> cancellationTokenOverrideScopeProvider)
        : base(cancellationTokenOverrideScopeProvider)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
