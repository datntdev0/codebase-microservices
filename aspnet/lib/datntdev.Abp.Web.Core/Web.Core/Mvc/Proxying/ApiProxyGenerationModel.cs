﻿using System.Linq;
using datntdev.Abp.Extensions;
using datntdev.Abp.Runtime.Validation;
using datntdev.Abp.Web.Api.ProxyScripting;
using datntdev.Abp.Web.Api.ProxyScripting.Generators.JQuery;

namespace datntdev.Abp.Web.Core.Mvc.Proxying;

public class ApiProxyGenerationModel : IShouldNormalize
{
    public string Type { get; set; }

    public bool UseCache { get; set; }

    public string Modules { get; set; }

    public string Controllers { get; set; }

    public string Actions { get; set; }

    public bool Minify { get; set; }

    public ApiProxyGenerationModel()
    {
        UseCache = true;
        Minify = false;
    }

    public void Normalize()
    {
        if (Type.IsNullOrEmpty())
        {
            Type = JQueryProxyScriptGenerator.Name;
        }
    }

    public ApiProxyGenerationOptions CreateOptions()
    {
        var options = new ApiProxyGenerationOptions(Type, UseCache);

        if (!Modules.IsNullOrEmpty())
        {
            options.Modules = Modules.Split('|').Select(m => m.Trim()).ToArray();
        }

        if (!Controllers.IsNullOrEmpty())
        {
            options.Controllers = Controllers.Split('|').Select(m => m.Trim()).ToArray();
        }

        if (!Actions.IsNullOrEmpty())
        {
            options.Actions = Actions.Split('|').Select(m => m.Trim()).ToArray();
        }

        return options;
    }
}
