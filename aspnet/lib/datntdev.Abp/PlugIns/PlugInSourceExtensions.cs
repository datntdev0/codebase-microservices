﻿using System;
using System.Collections.Generic;
using System.Linq;
using datntdev.Abp.Modules;

namespace datntdev.Abp.PlugIns
{
    public static class PlugInSourceExtensions
    {
        public static List<Type> GetModulesWithAllDependencies(this IPlugInSource plugInSource)
        {
            return plugInSource
                .GetModules()
                .SelectMany(AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule)
                .Distinct()
                .ToList();
        }
    }
}