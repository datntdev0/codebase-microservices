using datntdev.Abp.Web.Api.ProxyScripting.Configuration;
using datntdev.Abp.Web.MultiTenancy;
using datntdev.Abp.Web.Results.Filters;
using datntdev.Abp.Web.Security.AntiForgery;

namespace datntdev.Abp.Web.Configuration
{
    public class AbpWebCommonModuleConfiguration : IAbpWebCommonModuleConfiguration
    {
        public bool SendAllExceptionsToClients { get; set; }

        public IApiProxyScriptingConfiguration ApiProxyScripting { get; }

        public IAbpAntiForgeryConfiguration AntiForgery { get; }

        public IWebEmbeddedResourcesConfiguration EmbeddedResources { get; }

        public IWebMultiTenancyConfiguration MultiTenancy { get; }

        public WrapResultFilterCollection WrapResultFilters { get; }

        public AbpWebCommonModuleConfiguration(
            IApiProxyScriptingConfiguration apiProxyScripting,
            IAbpAntiForgeryConfiguration abpAntiForgery,
            IWebEmbeddedResourcesConfiguration embeddedResources,
            IWebMultiTenancyConfiguration multiTenancy)
        {
            ApiProxyScripting = apiProxyScripting;
            AntiForgery = abpAntiForgery;
            EmbeddedResources = embeddedResources;
            MultiTenancy = multiTenancy;
            WrapResultFilters = new WrapResultFilterCollection();
        }
    }
}