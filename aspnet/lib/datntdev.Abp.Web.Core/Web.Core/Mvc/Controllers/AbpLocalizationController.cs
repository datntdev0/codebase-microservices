using System;
using System.Collections.Generic;
using datntdev.Abp.Web.Core.Mvc.Extensions;
using datntdev.Abp.Auditing;
using datntdev.Abp.Configuration;
using datntdev.Abp.Localization;
using datntdev.Abp.Runtime.Caching;
using datntdev.Abp.Runtime.Session;
using datntdev.Abp.Timing;
using datntdev.Abp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using IUrlHelper = datntdev.Abp.Web.Http.IUrlHelper;

namespace datntdev.Abp.Web.Core.Mvc.Controllers;

public class AbpLocalizationController : AbpController
{
    protected IUrlHelper UrlHelper;
    private readonly ISettingStore _settingStore;

    private readonly ITypedCache<string, Dictionary<string, SettingInfo>> _userSettingCache;

    public AbpLocalizationController(
        IUrlHelper urlHelper,
        ISettingStore settingStore,
        ICacheManager cacheManager)
    {
        UrlHelper = urlHelper;
        _settingStore = settingStore;
        _userSettingCache = cacheManager.GetUserSettingsCache();
    }

    [DisableAuditing]
    public virtual ActionResult ChangeCulture(string cultureName, string returnUrl = "")
    {
        if (!GlobalizationHelper.IsValidCultureCode(cultureName))
        {
            throw new AbpException("Unknown language: " + cultureName + ". It must be a valid culture!");
        }

        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cultureName, cultureName));

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            cookieValue,
            new CookieOptions
            {
                Expires = Clock.Now.AddYears(2),
                HttpOnly = true
            }
        );

        if (AbpSession.UserId.HasValue)
        {
            ChangeCultureForUser(cultureName);
        }

        if (Request.IsAjaxRequest())
        {
            return Json(new AjaxResponse());
        }

        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            var escapedReturnUrl = Uri.EscapeDataString(returnUrl);
            var localPath = UrlHelper.LocalPathAndQuery(escapedReturnUrl, Request.Host.Host, Request.Host.Port);
            if (!string.IsNullOrWhiteSpace(localPath))
            {
                var unescapedLocalPath = Uri.UnescapeDataString(localPath);
                if (Url.IsLocalUrl(unescapedLocalPath))
                {
                    return LocalRedirect(unescapedLocalPath);
                }
            }
        }

        return LocalRedirect("/");
    }

    protected virtual void ChangeCultureForUser(string cultureName)
    {
        var languageSetting = _settingStore.GetSettingOrNull(
            AbpSession.TenantId,
            AbpSession.GetUserId(),
            LocalizationSettingNames.DefaultLanguage
        );

        if (languageSetting == null)
        {
            _settingStore.Create(new SettingInfo(
                AbpSession.TenantId,
                AbpSession.UserId,
                LocalizationSettingNames.DefaultLanguage,
                cultureName
            ));
        }
        else
        {
            _settingStore.Update(new SettingInfo(
                AbpSession.TenantId,
                AbpSession.UserId,
                LocalizationSettingNames.DefaultLanguage,
                cultureName
            ));
        }

        _userSettingCache.Remove(AbpSession.ToUserIdentifier().ToString());
    }
}
