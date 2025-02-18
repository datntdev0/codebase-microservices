using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.Runtime.Session;

namespace datntdev.Abp.Configuration
{
    public class RequiresAuthenticationSettingClientVisibilityProvider : ISettingClientVisibilityProvider
    {
        public async Task<bool> CheckVisible(IScopedIocResolver scope)
        {
            return await Task.FromResult(
                scope.Resolve<IAbpSession>().UserId.HasValue
            );
        }
    }
}