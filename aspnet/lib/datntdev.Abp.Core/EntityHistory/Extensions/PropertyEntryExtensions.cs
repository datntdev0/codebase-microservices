using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace datntdev.Abp.EntityHistory.Extensions;

public static class PropertyEntryExtensions
{
    public static object GetNewValue(this PropertyEntry propertyEntry)
    {
        if (propertyEntry.EntityEntry.State == EntityState.Deleted)
        {
            return null;
        }

        return propertyEntry.CurrentValue;
    }

    public static object GetOriginalValue(this PropertyEntry propertyEntry)
    {
        if (propertyEntry.EntityEntry.State == EntityState.Added)
        {
            return null;
        }

        return propertyEntry.OriginalValue;
    }
}