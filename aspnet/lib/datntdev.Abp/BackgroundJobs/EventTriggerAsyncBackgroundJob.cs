using System.Threading.Tasks;
using datntdev.Abp.Dependency;
using datntdev.Abp.Events.Bus;

namespace datntdev.Abp.BackgroundJobs
{
    public class EventTriggerAsyncBackgroundJob<TEventData> : IAsyncBackgroundJob<TEventData>, ITransientDependency
        where TEventData : EventData
    {
        public IEventBus EventBus { get; set; }

        public EventTriggerAsyncBackgroundJob()
        {
            EventBus = NullEventBus.Instance;
        }

        public async Task ExecuteAsync(TEventData eventData)
        {
            await EventBus.TriggerAsync(eventData);
        }
    }
}
