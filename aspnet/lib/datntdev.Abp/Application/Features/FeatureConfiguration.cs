using datntdev.Abp.Collections;

namespace datntdev.Abp.Application.Features
{
    /// <summary>
    /// public implementation for <see cref="IFeatureConfiguration"/>.
    /// </summary>
    public class FeatureConfiguration : IFeatureConfiguration
    {
        /// <summary>
        /// Reference to the feature providers.
        /// </summary>
        public ITypeList<FeatureProvider> Providers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureConfiguration"/> class.
        /// </summary>
        public FeatureConfiguration()
        {
            Providers = new TypeList<FeatureProvider>();
        }
    }
}