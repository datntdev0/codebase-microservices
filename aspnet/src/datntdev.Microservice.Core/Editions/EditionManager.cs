using datntdev.Abp.Application.Editions;
using datntdev.Abp.Application.Features;
using datntdev.Abp.Domain.Repositories;
using datntdev.Abp.Domain.Uow;

namespace datntdev.Microservice.Editions;

public class EditionManager : AbpEditionManager
{
    public EditionManager(
        IRepository<Edition> editionRepository,
        IAbpZeroFeatureValueStore featureValueStore,
        IUnitOfWorkManager unitOfWorkManager)
        : base(editionRepository, featureValueStore, unitOfWorkManager)
    {
    }
}
