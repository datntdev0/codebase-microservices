﻿using datntdev.Abp.Collections;

namespace datntdev.Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure setting system.
    /// </summary>
    public interface ISettingsConfiguration
    {
        /// <summary>
        /// List of settings providers.
        /// </summary>
        ITypeList<SettingProvider> Providers { get; }
        
        /// <summary>
        /// Setting encryption configuration
        /// </summary>
        SettingEncryptionConfiguration SettingEncryptionConfiguration { get; }
    }
}
