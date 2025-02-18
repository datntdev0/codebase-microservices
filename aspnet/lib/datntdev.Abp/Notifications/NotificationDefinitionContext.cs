namespace datntdev.Abp.Notifications
{
    public class NotificationDefinitionContext : INotificationDefinitionContext
    {
        public INotificationDefinitionManager Manager { get; private set; }

        public NotificationDefinitionContext(INotificationDefinitionManager manager)
        {
            Manager = manager;
        }
    }
}