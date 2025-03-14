﻿using datntdev.Abp.Dependency;
using datntdev.Abp.EntityHistory;
using datntdev.Abp.Runtime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;

namespace datntdev.Abp.Web.Core.EntityHistory;

/// <summary>
/// Implements <see cref="IEntityChangeSetReasonProvider"/> to get reason from HTTP request.
/// </summary>
public class HttpRequestEntityChangeSetReasonProvider : EntityChangeSetReasonProviderBase, ISingletonDependency
{
    [CanBeNull]
    public override string Reason => OverridedValue != null
        ? OverridedValue.Reason
        : HttpContextAccessor.HttpContext?.Request.GetDisplayUrl();

    protected IHttpContextAccessor HttpContextAccessor { get; }

    private const string SchemeDelimiter = "://";

    public HttpRequestEntityChangeSetReasonProvider(
        IHttpContextAccessor httpContextAccessor,

        IAmbientScopeProvider<ReasonOverride> reasonOverrideScopeProvider
        ) : base(reasonOverrideScopeProvider)
    {
        HttpContextAccessor = httpContextAccessor;
    }
}
