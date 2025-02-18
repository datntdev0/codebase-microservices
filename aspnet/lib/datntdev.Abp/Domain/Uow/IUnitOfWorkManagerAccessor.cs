namespace datntdev.Abp.Domain.Uow
{
    public interface IUnitOfWorkManagerAccessor
    {
        IUnitOfWorkManager UnitOfWorkManager { get; }
    }
}