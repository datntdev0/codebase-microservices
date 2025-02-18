using System;
using datntdev.Abp.Events.Bus.Handlers;
using datntdev.Abp.Reflection;

namespace datntdev.Abp.Events.Bus.Factories.Internals
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to handle events
    /// by a single instance object. 
    /// </summary>
    /// <remarks>
    /// This class always gets the same single instance of handler.
    /// </remarks>
    public class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// The event handler instance.
        /// </summary>
        public IEventHandler HandlerInstance { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        public Type GetHandlerType()
        {
            return ProxyHelper.UnProxy(HandlerInstance).GetType();
        }

        public void ReleaseHandler(IEventHandler handler)
        {
            
        }
    }
}