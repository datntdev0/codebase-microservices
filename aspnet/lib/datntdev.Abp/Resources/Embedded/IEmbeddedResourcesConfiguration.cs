using System.Collections.Generic;

namespace datntdev.Abp.Resources.Embedded
{
    public interface IEmbeddedResourcesConfiguration
    {
        List<EmbeddedResourceSet> Sources { get; }
    }
}