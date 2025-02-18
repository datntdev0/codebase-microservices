using datntdev.Abp.Domain.Uow;

namespace datntdev.Abp.EntityFramework
{
    public class AbpEfDbContextInitializationContext
    {
        public IUnitOfWork UnitOfWork { get; }

        public AbpEfDbContextInitializationContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
