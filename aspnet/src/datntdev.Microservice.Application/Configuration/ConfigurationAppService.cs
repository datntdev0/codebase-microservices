using datntdev.Abp.Authorization;
using datntdev.Abp.Runtime.Session;
using datntdev.Microservice.Configuration.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Configuration;

[AbpAuthorize]
public class ConfigurationAppService : MicroserviceAppServiceBase, IConfigurationAppService
{
    public async Task ChangeUiTheme(ChangeUiThemeInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
    }
}
