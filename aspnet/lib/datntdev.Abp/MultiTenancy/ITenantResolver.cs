using System.Threading.Tasks;

namespace datntdev.Abp.MultiTenancy
{
    public interface ITenantResolver
    {
        int? ResolveTenantId();
        
        Task<int?> ResolveTenantIdAsync();
    }
}
