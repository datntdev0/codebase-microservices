using datntdev.Microservice.Configuration.Dto;

namespace datntdev.Microservice.Configuration;

public interface IConfigAppService
{
    Task ChangeSettingAsync(ChangeSettingInput input);
}
