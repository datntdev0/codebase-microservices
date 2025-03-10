﻿using datntdev.Abp.Collections;

namespace datntdev.Abp.Notifications
{
    public class NotificationConfiguration : INotificationConfiguration
    {
        public ITypeList<NotificationProvider> Providers { get; private set; }

        public ITypeList<INotificationDistributer> Distributers { get; private set; }

        public ITypeList<IRealTimeNotifier> Notifiers { get; private set; }

        public NotificationConfiguration()
        {
            Providers = new TypeList<NotificationProvider>();
            Distributers = new TypeList<INotificationDistributer>();
            Notifiers = new TypeList<IRealTimeNotifier>();
        }
    }
}