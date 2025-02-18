using datntdev.Abp.Collections;

namespace datntdev.Abp.DynamicEntityProperties
{
    public interface IDynamicEntityPropertyConfiguration
    {
        ITypeList<DynamicEntityPropertyDefinitionProvider> Providers { get; }
    }
}
