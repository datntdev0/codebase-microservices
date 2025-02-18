using System;
using System.Linq;

namespace datntdev.Abp.Runtime.Validation
{
    public static class AbpValidationOptionsExtensions
    {
        public static bool IsConventionalValidationClass(this IAbpValidationDefaultOptions options, Type type)
        {
            return options.ConventionalValidationSelectors.Any(selector => selector(type));
        }
    }
}
