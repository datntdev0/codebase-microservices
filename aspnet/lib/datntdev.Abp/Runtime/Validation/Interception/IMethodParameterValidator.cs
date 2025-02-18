using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using datntdev.Abp.Dependency;

namespace datntdev.Abp.Runtime.Validation.Interception
{
    /// <summary>
    /// This interface is used to validate method parameters.
    /// </summary>
    public interface IMethodParameterValidator : ITransientDependency
    {
        IReadOnlyList<ValidationResult> Validate(object validatingObject);
    }
}
