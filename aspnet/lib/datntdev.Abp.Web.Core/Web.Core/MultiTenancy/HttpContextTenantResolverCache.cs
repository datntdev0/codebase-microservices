﻿using datntdev.Abp.Dependency;
using datntdev.Abp.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace datntdev.Abp.Web.Core.MultiTenancy;

public class HttpContextTenantResolverCache : ITenantResolverCache, ITransientDependency
{
    private const string CacheItemKey = "Abp.MultiTenancy.TenantResolverCacheItem";

    public TenantResolverCacheItem Value
    {
        get
        {
            return _httpContextAccessor.HttpContext?.Items[CacheItemKey] as TenantResolverCacheItem;
        }

        set
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            httpContext.Items[CacheItemKey] = value;
        }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantResolverCache(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}
