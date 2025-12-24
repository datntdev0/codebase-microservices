using datntdev.Microservices.Common.Application;

namespace datntdev.Microservices.Common.Web.App.Application
{
    /// <summary>
    /// Represents a base class for application providers that serve as singleton services 
    /// to provide static and immutable information for business logic. 
    /// Examples include permission providers, localization providers, feature providers, etc.
    /// </summary>
    public abstract class BaseAppProvider : IAppProvider
    {
    }
}
