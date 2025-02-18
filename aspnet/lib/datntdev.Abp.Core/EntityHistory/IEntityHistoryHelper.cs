using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace datntdev.Abp.EntityHistory;

public interface IEntityHistoryHelper
{
    EntityChangeSet CreateEntityChangeSet(ICollection<EntityEntry> entityEntries);

    Task SaveAsync(EntityChangeSet changeSet);

    void Save(EntityChangeSet changeSet);
}