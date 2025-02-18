namespace datntdev.Abp.Configuration.Startup
{
    public class ModuleConfigurations : IModuleConfigurations
    {
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        public ModuleConfigurations(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;
        }
    }
}