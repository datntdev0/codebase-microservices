﻿using System;
using System.Globalization;
using datntdev.Abp.Configuration;
using datntdev.Abp.Dependency;
using datntdev.Abp.Localization;
using datntdev.Abp.Localization.Sources;
using datntdev.Abp.ObjectMapping;
using datntdev.Abp.Runtime.Session;
using Castle.Core.Logging;
using Microsoft.AspNetCore.SignalR;

namespace datntdev.Abp.Web.Core.SignalR.Hubs;

public abstract class AbpHubBase : Hub
{
    public ILogger Logger { get; set; }

    [Obsolete("Use Context.User instead.")]
    public IAbpSession AbpSession { get; set; }

    public IIocResolver IocResolver { get; set; }

    public IObjectMapper ObjectMapper { get; set; }

    public ISettingManager SettingManager { get; set; }

    public ILocalizationManager LocalizationManager { get; set; }

    /// <summary>
    /// Gets/sets name of the localization source that is used in this application service.
    /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
    /// </summary>
    protected string LocalizationSourceName { get; set; }

    /// <summary>
    /// Gets localization source.
    /// It's valid if <see cref="LocalizationSourceName"/> is set.
    /// </summary>
    protected ILocalizationSource LocalizationSource
    {
        get
        {
            if (LocalizationSourceName == null)
            {
                throw new AbpException("Must set LocalizationSourceName before, in order to get LocalizationSource");
            }

            if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
            {
                _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);
            }

            return _localizationSource;
        }
    }
    private ILocalizationSource _localizationSource;

    protected bool Disposed { get; private set; }

    protected AbpHubBase()
    {
        Logger = NullLogger.Instance;
        ObjectMapper = NullObjectMapper.Instance;
        LocalizationManager = NullLocalizationManager.Instance;
    }

    /// <summary>
    /// Gets localized string for given key name and current language.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name)
    {
        return LocalizationSource.GetString(name);
    }

    /// <summary>
    /// Gets localized string for given key name and current language with formatting strings.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="args">Format arguments</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, params object[] args)
    {
        return LocalizationSource.GetString(name, args);
    }

    /// <summary>
    /// Gets localized string for given key name and specified culture information.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="culture">culture information</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, CultureInfo culture)
    {
        return LocalizationSource.GetString(name, culture);
    }

    /// <summary>
    /// Gets localized string for given key name and current language with formatting strings.
    /// </summary>
    /// <param name="name">Key name</param>
    /// <param name="culture">culture information</param>
    /// <param name="args">Format arguments</param>
    /// <returns>Localized string</returns>
    protected virtual string L(string name, CultureInfo culture, params object[] args)
    {
        return LocalizationSource.GetString(name, culture, args);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (Disposed)
        {
            return;
        }

        if (disposing)
        {
            Disposed = true;
            IocResolver?.Release(this);
        }
    }
}
