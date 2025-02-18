using System.Threading.Tasks;
using datntdev.Abp.Dependency;

namespace datntdev.Abp.Configuration
{
    public interface ISettingClientVisibilityProvider
    {
        Task<bool> CheckVisible(IScopedIocResolver scope);
    }
}