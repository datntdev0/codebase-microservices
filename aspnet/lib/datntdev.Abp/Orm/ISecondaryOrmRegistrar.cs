using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;

namespace datntdev.Abp.Orm
{
    public interface ISecondaryOrmRegistrar
    {
        string OrmContextKey { get; }

        void RegisterRepositories(IIocManager iocManager, AutoRepositoryTypesAttribute defaultRepositoryTypes);
    }
}
