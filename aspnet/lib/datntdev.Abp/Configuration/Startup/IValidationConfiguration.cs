using System;
using System.Collections.Generic;
using datntdev.Abp.Collections;
using datntdev.Abp.Runtime.Validation.Interception;

namespace datntdev.Abp.Configuration.Startup
{
    public interface IValidationConfiguration
    {
        List<Type> IgnoredTypes { get; }

        /// <summary>
        /// A list of method parameter validators.
        /// </summary>
        ITypeList<IMethodParameterValidator> Validators { get; }
    }
}