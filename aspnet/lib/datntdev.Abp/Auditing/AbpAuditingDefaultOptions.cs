using System;
using System.Collections.Generic;
using System.Linq;
using datntdev.Abp.Application.Services;

namespace datntdev.Abp.Auditing
{
    public class AbpAuditingDefaultOptions : IAbpAuditingDefaultOptions
    {
        public static List<Func<Type, bool>> ConventionalAuditingSelectorList = new List<Func<Type, bool>>
        {
            type => typeof(IApplicationService).IsAssignableFrom(type)
        };

        public List<Func<Type, bool>> ConventionalAuditingSelectors { get; }

        public AbpAuditingDefaultOptions()
        {
            ConventionalAuditingSelectors = ConventionalAuditingSelectorList.ToList();
        }
    }
}
