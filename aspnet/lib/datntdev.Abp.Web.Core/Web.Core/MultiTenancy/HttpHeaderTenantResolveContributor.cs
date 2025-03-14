﻿using System.Linq;
using datntdev.Abp.Collections.Extensions;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.MultiTenancy;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.MultiTenancy;

public class HttpHeaderTenantResolveContributor : ITenantResolveContributor, ITransientDependency
{
    public ILogger Logger { get; set; }

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMultiTenancyConfig _multiTenancyConfig;

    public HttpHeaderTenantResolveContributor(
        IHttpContextAccessor httpContextAccessor,
        IMultiTenancyConfig multiTenancyConfig)
    {
        _httpContextAccessor = httpContextAccessor;
        _multiTenancyConfig = multiTenancyConfig;

        Logger = NullLogger.Instance;
    }

    public int? ResolveTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        var tenantIdHeader = httpContext.Request.Headers[_multiTenancyConfig.TenantIdResolveKey];
        if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
        {
            return null;
        }

        if (tenantIdHeader.Count > 1)
        {
            Logger.Warn(
                $"HTTP request includes more than one {_multiTenancyConfig.TenantIdResolveKey} header value. First one will be used. All of them: {tenantIdHeader.JoinAsString(", ")}"
                );
        }

        return int.TryParse(tenantIdHeader.First(), out var tenantId) ? tenantId : (int?)null;
    }
}
