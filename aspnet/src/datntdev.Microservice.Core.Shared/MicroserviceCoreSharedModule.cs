using datntdev.Abp.Modules;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Runtime.Security;

namespace datntdev.Microservice
{
    public class MicroserviceCoreSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = MicroserviceConsts.MultiTenancyEnabled;
            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = MicroserviceConsts.DefaultPassPhrase;

            SimpleStringCipher.DefaultPassPhrase = MicroserviceConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MicroserviceCoreSharedModule).GetAssembly());
        }
    }
}
