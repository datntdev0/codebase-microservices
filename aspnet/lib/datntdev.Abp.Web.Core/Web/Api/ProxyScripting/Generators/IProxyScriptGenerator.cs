using datntdev.Abp.Web.Api.Modeling;

namespace datntdev.Abp.Web.Api.ProxyScripting.Generators
{
    public interface IProxyScriptGenerator
    {
        string CreateScript(ApplicationApiDescriptionModel model);
    }
}