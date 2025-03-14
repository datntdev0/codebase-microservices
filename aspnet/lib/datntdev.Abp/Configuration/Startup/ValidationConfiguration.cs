﻿using System;
using System.Collections.Generic;
using datntdev.Abp.Collections;
using datntdev.Abp.Runtime.Validation.Interception;

namespace datntdev.Abp.Configuration.Startup
{
    public class ValidationConfiguration : IValidationConfiguration
    {
        public List<Type> IgnoredTypes { get; }

        public ITypeList<IMethodParameterValidator> Validators { get; }

        public ValidationConfiguration()
        {
            IgnoredTypes = new List<Type>();
            Validators = new TypeList<IMethodParameterValidator>();
        }
    }
}