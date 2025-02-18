using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace datntdev.Abp.Web.Core.PlugIn;

public class AbpPlugInAssemblyPart : AssemblyPart, ICompilationReferencesProvider
{
    public AbpPlugInAssemblyPart(Assembly assembly)
        : base(assembly)
    {
    }

    IEnumerable<string> ICompilationReferencesProvider.GetReferencePaths() => Enumerable.Empty<string>();
}
