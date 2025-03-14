using System;
using System.Collections.Generic;
using System.Linq;
using datntdev.Abp.Application.Services;

namespace datntdev.Abp.Runtime.Validation
{
    public class AbpValidationDefaultOptions : IAbpValidationDefaultOptions
    {
        public static List<Func<Type, bool>> ConventionalValidationSelectorList = new List<Func<Type, bool>>
        {
            type => typeof(IApplicationService).IsAssignableFrom(type)
        };
        
        public List<Func<Type, bool>> ConventionalValidationSelectors { get; }

        public AbpValidationDefaultOptions()
        {
            ConventionalValidationSelectors = ConventionalValidationSelectorList.ToList();
        }
    }
}
