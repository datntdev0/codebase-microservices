using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using datntdev.Abp.Domain.Entities;
using datntdev.Abp.Extensions;

namespace datntdev.Abp.EntityHistory.Extensions;

public static class EntityEntryExtensions
{
    public static bool IsCreated(this EntityEntry entityEntry)
    {
        return entityEntry.State == EntityState.Added;
    }

    public static bool IsDeleted(this EntityEntry entityEntry)
    {
        if (entityEntry.State == EntityState.Deleted)
        {
            return true;
        }
        var entity = entityEntry.Entity;
        return entity is ISoftDelete && entity.As<ISoftDelete>().IsDeleted;
    }
}