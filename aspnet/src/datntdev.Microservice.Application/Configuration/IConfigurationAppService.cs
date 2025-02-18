using datntdev.Microservice.Configuration.Dto;
using System.Threading.Tasks;

namespace datntdev.Microservice.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
