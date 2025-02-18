using System;
using System.Threading.Tasks;
using datntdev.Abp.Events.Bus;

namespace datntdev.Abp.BackgroundJobs
{
    public static class BackgroundJobManagerEventTriggerExtensions
    {
        public static Task EnqueueEventAsync<TEventData>(
            this IBackgroundJobManager backgroundJobManager,
            TEventData eventData,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null
          )
          where TEventData : EventData
        {
            return backgroundJobManager.EnqueueAsync<EventTriggerAsyncBackgroundJob<TEventData>,TEventData>(eventData, priority, delay);
        }
    }
}