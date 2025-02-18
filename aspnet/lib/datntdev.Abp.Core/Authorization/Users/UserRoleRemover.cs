using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;

namespace datntdev.Abp.Authorization.Users
{
    /// <summary>
    /// Removes the user from all user roles when a user is deleted.
    /// </summary>
    public class UserRoleRemover :
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,
        ITransientDependency
    {
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserRoleRemover(
            IUnitOfWorkManager unitOfWorkManager, 
            IRepository<UserRole, long> userRoleRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _userRoleRepository = userRoleRepository;
        }
        
        public virtual void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            _unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
                {
                    _userRoleRepository.Delete(
                        ur => ur.UserId == eventData.Entity.Id
                    );
                }
            });
        }
    }
}
