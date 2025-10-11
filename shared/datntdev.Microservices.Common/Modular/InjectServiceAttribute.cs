using Microsoft.Extensions.DependencyInjection;

namespace datntdev.Microservices.Common.Modular
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class InjectServiceAttribute(ServiceLifetime lifetime) : Attribute
    {
        public ServiceLifetime Lifetime { get; } = lifetime;
    }
}
