namespace datntdev.Abp.Configuration.Startup
{
    public class EventBusConfiguration : IEventBusConfiguration
    {
        public bool UseDefaultEventBus { get; set; }

        public EventBusConfiguration()
        {
            UseDefaultEventBus = true;
        }
    }
}