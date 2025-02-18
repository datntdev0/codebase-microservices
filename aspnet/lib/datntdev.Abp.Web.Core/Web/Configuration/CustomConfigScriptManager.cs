using System.Text;
using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Dependency;
using datntdev.Abp.Json;

namespace datntdev.Abp.Web.Configuration
{
    public class CustomConfigScriptManager : ICustomConfigScriptManager, ITransientDependency
    {
        private readonly IAbpStartupConfiguration _abpStartupConfiguration;

        public CustomConfigScriptManager(IAbpStartupConfiguration abpStartupConfiguration)
        {
            _abpStartupConfiguration = abpStartupConfiguration;
        }

        public string GetScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(abp){");
            script.AppendLine();

            script.AppendLine("    abp.custom = " + _abpStartupConfiguration.GetCustomConfig().ToJsonString());

            script.AppendLine();
            script.Append("})(abp);");

            return script.ToString();
        }
    }
}