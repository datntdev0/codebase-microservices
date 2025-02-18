using System;
using System.Collections.Generic;

namespace datntdev.Abp.Auditing
{
    public interface IAbpAuditingDefaultOptions
    {
        /// <summary>
        /// A list of selectors to determine conventional Auditing classes.
        /// </summary>
        List<Func<Type, bool>> ConventionalAuditingSelectors { get; }
    }
}
