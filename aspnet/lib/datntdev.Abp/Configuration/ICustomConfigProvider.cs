using System.Collections.Generic;

namespace datntdev.Abp.Configuration.Startup
{
    public interface ICustomConfigProvider
    {
        Dictionary<string, object> GetConfig(CustomConfigProviderContext customConfigProviderContext);
    }
}
