using datntdev.Abp.Configuration.Startup;
using datntdev.Abp.Localization.Dictionaries;
using datntdev.Abp.Localization.Dictionaries.Xml;
using datntdev.Abp.Modules;
using datntdev.Abp.Web.Api.ProxyScripting.Configuration;
using datntdev.Abp.Web.Api.ProxyScripting.Generators.JQuery;
using datntdev.Abp.Web.Configuration;
using datntdev.Abp.Web.MultiTenancy;
using datntdev.Abp.Web.Security.AntiForgery;
using datntdev.Abp.Reflection.Extensions;
using datntdev.Abp.Web.Minifier;

namespace datntdev.Abp.Web
{
    /// <summary>
    /// This module is used to use ABP in ASP.NET web applications.
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]    
    public class AbpWebCommonModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.Register<IWebMultiTenancyConfiguration, WebMultiTenancyConfiguration>();
            IocManager.Register<IApiProxyScriptingConfiguration, ApiProxyScriptingConfiguration>();
            IocManager.Register<IAbpAntiForgeryConfiguration, AbpAntiForgeryConfiguration>();
            IocManager.Register<IWebEmbeddedResourcesConfiguration, WebEmbeddedResourcesConfiguration>();
            IocManager.Register<IAbpWebCommonModuleConfiguration, AbpWebCommonModuleConfiguration>();
            IocManager.Register<IJavaScriptMinifier, NUglifyJavaScriptMinifier>();

            Configuration.Modules.AbpWebCommon().ApiProxyScripting.Generators[JQueryProxyScriptGenerator.Name] = typeof(JQueryProxyScriptGenerator);

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpWebConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AbpWebCommonModule).GetAssembly()
                    )
                )
            );
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpWebCommonModule).GetAssembly());            
        }
    }
}
