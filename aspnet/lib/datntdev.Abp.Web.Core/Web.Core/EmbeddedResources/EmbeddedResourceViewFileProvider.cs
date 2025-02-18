using datntdev.Abp.Dependency;
using datntdev.Abp.Resources.Embedded;

namespace datntdev.Abp.Web.Core.EmbeddedResources;

public class EmbeddedResourceViewFileProvider : EmbeddedResourceFileProvider
{
    public EmbeddedResourceViewFileProvider(IIocResolver iocResolver)
        : base(iocResolver)
    {
    }

    protected override bool IsIgnoredFile(EmbeddedResourceItem resource)
    {
        return resource.FileExtension != "cshtml";
    }
}
