using datntdev.Abp.Authorization.Roles;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Events.Bus.Handlers;

namespace datntdev.Abp.Organizations
{
    /// <summary>
    /// Removes the role from all organization units when a role is deleted.
    /// </summary>
    public class OrganizationUnitRoleRemover : 
        IEventHandler<EntityDeletedEventData<AbpRoleBase>>, 
        ITransientDependency
    {
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrganizationUnitRoleRemover(
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository, 
            IUnitOfWorkManager unitOfWorkManager)
        {
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
        public virtual void HandleEvent(EntityDeletedEventData<AbpRoleBase> eventData)
        {
            _unitOfWorkManager.WithUnitOfWork(() =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(eventData.Entity.TenantId))
                {
                    _organizationUnitRoleRepository.Delete(
                        uou => uou.RoleId == eventData.Entity.Id
                    );
                }
            });
        }
    }
}
