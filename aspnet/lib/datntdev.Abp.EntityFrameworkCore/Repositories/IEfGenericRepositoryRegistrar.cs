using System;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Repositories;

namespace datntdev.Abp.EntityFramework.Repositories
{
    public interface IEfGenericRepositoryRegistrar
    {
        void RegisterForDbContext(
            Type dbContextType,
            IIocManager iocManager,
            AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute
        );

        void RegisterForEntity(
            Type dbContextType,
            Type entityType,
            IIocManager iocManager,
            AutoRepositoryTypesAttribute defaultAutoRepositoryTypesAttribute
        );
    }
}
