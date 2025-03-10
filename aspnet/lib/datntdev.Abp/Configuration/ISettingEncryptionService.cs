﻿using JetBrains.Annotations;

namespace datntdev.Abp.Configuration
{
    public interface ISettingEncryptionService
    {
        [CanBeNull]
        string Encrypt([NotNull]SettingDefinition settingDefinition, [CanBeNull] string plainValue);

        [CanBeNull]
        string Decrypt([NotNull]SettingDefinition settingDefinition, [CanBeNull] string encryptedValue);
    }
}