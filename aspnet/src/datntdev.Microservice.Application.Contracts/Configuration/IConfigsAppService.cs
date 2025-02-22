using datntdev.Microservice.Configuration.Dto;

namespace datntdev.Microservice.Configuration;

public interface IConfigsAppService
{
    Task UpdateAsync(ChangeSettingInput input);
}
