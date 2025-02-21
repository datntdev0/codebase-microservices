using datntdev.Abp.Configuration;
using System.Collections.Generic;

namespace datntdev.Microservice.Configuration;

public class AppSettingProvider : SettingProvider
{
    public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
    {
        return
        [
            new(AppSettingNames.AppExampleSetting, "default-value",
                scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User,
                clientVisibilityProvider: new VisibleSettingClientVisibilityProvider())
        ];
    }
}
