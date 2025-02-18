using System.Reflection;
using datntdev.Abp.Dependency;
using Castle.Core;

namespace datntdev.Abp.Runtime.Validation.Interception
{
    public static class ValidationInterceptorRegistrar
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            {
                var implementationType = handler.ComponentModel.Implementation.GetTypeInfo();
            
                if (!iocManager.IsRegistered<IAbpValidationDefaultOptions>())
                {
                    return;
                }
                
                var validationOptions = iocManager.Resolve<IAbpValidationDefaultOptions>();

                if (validationOptions.IsConventionalValidationClass(implementationType.AsType()))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<ValidationInterceptor>)));
                }
            };
        }
    }
}
