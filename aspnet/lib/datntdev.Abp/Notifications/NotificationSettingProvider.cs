using System.Collections.Generic;
using datntdev.Abp.Configuration;
using datntdev.Abp.Localization;

namespace datntdev.Abp.Notifications
{
    public class NotificationSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    NotificationSettingNames.ReceiveNotifications,
                    "true",
                    L("ReceiveNotifications"),
                    scopes: SettingScopes.User,
                    clientVisibilityProvider: new VisibleSettingClientVisibilityProvider())
            };
        }

        private static LocalizableString L(string name)
        {
            return new LocalizableString(name, AbpConsts.LocalizationSourceName);
        }
    }
}
