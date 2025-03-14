﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Extensions;
using datntdev.Abp.Logging;
using Castle.Core.Logging;

namespace datntdev.Abp.Localization
{
    public static class LocalizationSourceHelper
    {
        public static string ReturnGivenNameOrThrowException(
            ILocalizationConfiguration configuration,
            string sourceName,
            string name,
            CultureInfo culture,
            ILogger logger = null)
        {
            var exceptionMessage = $"Can not find '{name}' in localization source '{sourceName}'!";

            if (!configuration.ReturnGivenTextIfNotFound)
            {
                throw new AbpException(exceptionMessage);
            }

            if (configuration.LogWarnMessageIfNotFound)
            {
                (logger ?? LogHelper.Logger).Warn(exceptionMessage);
            }

            var notFoundText = configuration.HumanizeTextIfNotFound
                ? name.ToSentenceCase(culture)
                : name;

            return configuration.WrapGivenTextIfNotFound
                ? $"[{notFoundText}]"
                : notFoundText;
        }

        public static List<string> ReturnGivenNamesOrThrowException(
            ILocalizationConfiguration configuration,
            string sourceName,
            List<string> names,
            CultureInfo culture,
            ILogger logger = null)
        {
            var exceptionMessage = $"Can not find '{string.Join(",", names)}' in localization source '{sourceName}' with culture '{culture.Name}'!";

            if (!configuration.ReturnGivenTextIfNotFound)
            {
                throw new AbpException(exceptionMessage);
            }

            if (configuration.LogWarnMessageIfNotFound)
            {
                (logger ?? LogHelper.Logger).Warn(exceptionMessage);
            }

            var notFoundText = configuration.HumanizeTextIfNotFound
                ? names.Select(name => name.ToSentenceCase(culture)).ToList()
                : names;

            return configuration.WrapGivenTextIfNotFound
                ? notFoundText.Select(text => $"[{text}]").ToList()
                : notFoundText;
        }
    }
}
