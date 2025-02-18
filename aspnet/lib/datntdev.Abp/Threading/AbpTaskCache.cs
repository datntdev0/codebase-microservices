using System.Threading.Tasks;

namespace datntdev.Abp.Threading
{
    public static class AbpTaskCache
    {
        public static Task CompletedTask { get; } = Task.FromResult(0);
    }
}
