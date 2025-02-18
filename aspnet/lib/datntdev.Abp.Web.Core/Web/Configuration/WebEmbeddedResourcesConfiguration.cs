using System.Collections.Generic;

namespace datntdev.Abp.Web.Configuration
{
    public class WebEmbeddedResourcesConfiguration : IWebEmbeddedResourcesConfiguration
    {
        public HashSet<string> IgnoredFileExtensions { get; }

        public WebEmbeddedResourcesConfiguration()
        {
            IgnoredFileExtensions = new HashSet<string>
            {
                "cshtml",
                "config"
            };
        }
    }
}