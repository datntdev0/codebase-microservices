using System;
using System.Reflection;

namespace datntdev.Abp.Aspects
{
    //THIS NAMESPACE IS WORK-IN-PROGRESS

    public abstract class AspectAttribute : Attribute
    {
        public Type InterceptorType { get; set; }

        protected AspectAttribute(Type interceptorType)
        {
            InterceptorType = interceptorType;
        }
    }

    public interface IAbpInterceptionContext
    {
        object Target { get; }

        MethodInfo Method { get; }

        object[] Arguments { get; }

        object ReturnValue { get; }

        bool Handled { get; set; }
    }

    public interface IAbpBeforeExecutionInterceptionContext : IAbpInterceptionContext
    {

    }


    public interface IAbpAfterExecutionInterceptionContext : IAbpInterceptionContext
    {
        Exception Exception { get; }
    }

    public interface IAbpInterceptor<TAspect>
    {
        TAspect Aspect { get; set; }

        void BeforeExecution(IAbpBeforeExecutionInterceptionContext context);

        void AfterExecution(IAbpAfterExecutionInterceptionContext context);
    }

    public abstract class AbpInterceptorBase<TAspect> : IAbpInterceptor<TAspect>
    {
        public TAspect Aspect { get; set; }

        public virtual void BeforeExecution(IAbpBeforeExecutionInterceptionContext context)
        {
        }

        public virtual void AfterExecution(IAbpAfterExecutionInterceptionContext context)
        {
        }
    }

    public class Test_Aspects
    {
        public class MyAspectAttribute : AspectAttribute
        {
            public int TestValue { get; set; }

            public MyAspectAttribute()
                : base(typeof(MyInterceptor))
            {
            }
        }

        public class MyInterceptor : AbpInterceptorBase<MyAspectAttribute>
        {
            public override void BeforeExecution(IAbpBeforeExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }

            public override void AfterExecution(IAbpAfterExecutionInterceptionContext context)
            {
                Aspect.TestValue++;
            }
        }

        public class MyService
        {
            [MyAspect(TestValue = 41)] //Usage!
            public void DoIt()
            {

            }
        }

        public class MyClient
        {
            private readonly MyService _service;

            public MyClient(MyService service)
            {
                _service = service;
            }

            public void Test()
            {
                _service.DoIt();
            }
        }
    }
}
