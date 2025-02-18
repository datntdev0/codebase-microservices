using System;
using System.Collections.Generic;
using datntdev.Abp.Domain.Entities;

namespace datntdev.Abp.EntityFramework
{
    public interface IDbContextEntityFinder
    {
        IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType);
    }
}