using datntdev.Abp.Web.Core.Mvc.Controllers;
using datntdev.Abp.Auditing;
using datntdev.Abp.Web.Api.ProxyScripting;
using datntdev.Abp.Web.Minifier;
using datntdev.Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace datntdev.Abp.Web.Core.Mvc.Proxying;

[DontWrapResult]
[DisableAuditing]
public class AbpServiceProxiesController : AbpController
{
    private readonly IApiProxyScriptManager _proxyScriptManager;
    private readonly IJavaScriptMinifier _javaScriptMinifier;

    public AbpServiceProxiesController(IApiProxyScriptManager proxyScriptManager,
        IJavaScriptMinifier javaScriptMinifier)
    {
        _proxyScriptManager = proxyScriptManager;
        _javaScriptMinifier = javaScriptMinifier;
    }

    [Produces("application/x-javascript")]
    public ContentResult GetAll(ApiProxyGenerationModel model)
    {
        var script = _proxyScriptManager.GetScript(model.CreateOptions());
        return Content(model.Minify ? _javaScriptMinifier.Minify(script) : script, "application/x-javascript");
    }
}
