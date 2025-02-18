using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Entities;
using datntdev.Abp.EntityFramework;
using datntdev.Abp.Reflection;
using Microsoft.EntityFrameworkCore;

namespace datntdev.Abp.EntityFrameworkCore;

public class EfCoreDbContextEntityFinder : IDbContextEntityFinder, ITransientDependency
{
    public IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
    {
        return
            from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            where
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
                ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0],
                    typeof(IEntity<>))
            select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
    }
}