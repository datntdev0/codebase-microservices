using System;
using datntdev.Abp.Authorization.Users;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;

namespace datntdev.Abp.Notifications
{
    public class NotificationSubscriptionSynchronizer : IEventHandler<EntityDeletedEventData<AbpUserBase>>,
        ITransientDependency
    {
        private readonly IRepository<NotificationSubscriptionInfo, Guid> _notificationSubscriptionRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NotificationSubscriptionSynchronizer(
            IRepository<NotificationSubscriptionInfo, Guid> notificationSubscriptionRepository,
            IUnitOfWorkManager unitOfWorkManager
        )
        {
            _notificationSubscriptionRepository = notificationSubscriptionRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            _unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
                {
                    _notificationSubscriptionRepository.Delete(x => x.UserId == eventData.Entity.Id);
                }
            });
        }
    }
}
