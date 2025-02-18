using System;

using datntdev.Abp.EntityFramework;

namespace datntdev.Abp.EntityFrameworkCore;

public class EfCoreBasedSecondaryOrmRegistrar : SecondaryOrmRegistrarBase
{
    public EfCoreBasedSecondaryOrmRegistrar(Type dbContextType, IDbContextEntityFinder dbContextEntityFinder)
        : base(dbContextType, dbContextEntityFinder)
    {
    }

    public override string OrmContextKey { get; } = AbpConsts.Orms.EntityFrameworkCore;
}