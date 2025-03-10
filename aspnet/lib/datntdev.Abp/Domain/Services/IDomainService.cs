using datntdev.Abp.Dependency;

namespace datntdev.Abp.Domain.Services
{
    /// <summary>
    /// This interface must be implemented by all domain services to identify them by convention.
    /// </summary>
    public interface IDomainService : ITransientDependency
    {

    }
}