using datntdev.Abp.Authorization;
using datntdev.Abp.Runtime.Session;
using datntdev.Microservice.Configuration.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Configuration;

[AbpAuthorize]
public class ConfigAppService : MicroserviceAppServiceBase, IConfigAppService
{
    public async Task ChangeSettingAsync(ChangeSettingInput input)
    {
        await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.AppExampleSetting, input.Value);
    }
}
