using Castle.DynamicProxy;

namespace datntdev.Abp.Dependency
{
    public class AbpAsyncDeterminationInterceptor<TInterceptor> : AsyncDeterminationInterceptor
        where TInterceptor : IAsyncInterceptor
    {
        public AbpAsyncDeterminationInterceptor(TInterceptor asyncInterceptor) : base(asyncInterceptor)
        {

        }
    }
}