using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using datntdev.Abp.Dependency;
using datntdev.Abp.Domain.Uow;
using datntdev.Abp.EntityHistory.Extensions;
using datntdev.Abp.Events.Bus.Entities;
using datntdev.Abp.Extensions;
using datntdev.Abp.Json;
using datntdev.Abp.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace datntdev.Abp.EntityHistory;

public class EntityHistoryHelper : EntityHistoryHelperBase, IEntityHistoryHelper, ITransientDependency
{
    public EntityHistoryHelper(
        IEntityHistoryConfiguration configuration,
        IUnitOfWorkManager unitOfWorkManager)
        : base(configuration, unitOfWorkManager)
    {
    }

    public virtual EntityChangeSet CreateEntityChangeSet(ICollection<EntityEntry> entityEntries)
    {
        var changeSet = new EntityChangeSet
        {
            Reason = EntityChangeSetReasonProvider.Reason.TruncateWithPostfix(EntityChangeSet.MaxReasonLength),

            // Fill "who did this change"
            BrowserInfo = ClientInfoProvider.BrowserInfo.TruncateWithPostfix(EntityChangeSet.MaxBrowserInfoLength),
            ClientIpAddress =
                ClientInfoProvider.ClientIpAddress.TruncateWithPostfix(EntityChangeSet.MaxClientIpAddressLength),
            ClientName = ClientInfoProvider.ComputerName.TruncateWithPostfix(EntityChangeSet.MaxClientNameLength),
            ImpersonatorTenantId = AbpSession.ImpersonatorTenantId,
            ImpersonatorUserId = AbpSession.ImpersonatorUserId,
            TenantId = AbpSession.TenantId,
            UserId = AbpSession.UserId
        };

        if (!IsEntityHistoryEnabled)
        {
            return changeSet;
        }

        foreach (var entityEntry in entityEntries)
        {
            var shouldSaveEntityHistory = ShouldSaveEntityHistory(entityEntry);
            if (shouldSaveEntityHistory.HasValue && !shouldSaveEntityHistory.Value)
            {
                continue;
            }

            var entityChange = CreateEntityChange(entityEntry);
            if (entityChange == null)
            {
                continue;
            }

            var shouldSaveAuditedPropertiesOnly = !shouldSaveEntityHistory.HasValue;
            var propertyChanges = GetPropertyChanges(entityEntry, shouldSaveAuditedPropertiesOnly);
            if (propertyChanges.Count == 0)
            {
                continue;
            }

            entityChange.PropertyChanges = propertyChanges;
            changeSet.EntityChanges.Add(entityChange);
        }

        return changeSet;
    }

    public virtual async Task SaveAsync(EntityChangeSet changeSet)
    {
        if (!IsEntityHistoryEnabled)
        {
            return;
        }

        UpdateChangeSet(changeSet);

        if (changeSet.EntityChanges.Count == 0)
        {
            return;
        }

        using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        {
            await EntityHistoryStore.SaveAsync(changeSet);
            await uow.CompleteAsync();
        }
    }

    public virtual void Save(EntityChangeSet changeSet)
    {
        if (!IsEntityHistoryEnabled)
        {
            return;
        }

        UpdateChangeSet(changeSet);

        if (changeSet.EntityChanges.Count == 0)
        {
            return;
        }

        using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
        {
            EntityHistoryStore.Save(changeSet);
            uow.Complete();
        }
    }

    protected virtual string GetEntityId(EntityEntry entry)
    {
        var primaryKeys = entry.Properties.Where(p => p.Metadata.IsPrimaryKey());
        return primaryKeys.First().CurrentValue?.ToJsonString();
    }

    protected virtual bool? ShouldSaveEntityHistory(EntityEntry entityEntry)
    {
        if (entityEntry.State == EntityState.Detached ||
            entityEntry.State == EntityState.Unchanged)
        {
            return false;
        }

        var typeOfEntity = ProxyHelper.GetUnproxiedType(entityEntry.Entity);
        var shouldTrackEntity = IsTypeOfTrackedEntity(typeOfEntity);
        if (shouldTrackEntity.HasValue && !shouldTrackEntity.Value)
        {
            return false;
        }

        if (!IsTypeOfEntity(typeOfEntity) && !entityEntry.Metadata.IsOwned())
        {
            return false;
        }

        var shouldAuditEntity = IsTypeOfAuditedEntity(typeOfEntity);
        if (shouldAuditEntity.HasValue && !shouldAuditEntity.Value)
        {
            return false;
        }

        bool? shouldAuditOwnerEntity = null;
        bool? shouldAuditOwnerProperty = null;
        if (!shouldAuditEntity.HasValue && entityEntry.Metadata.IsOwned())
        {
            // Check if owner entity has auditing attribute
            var ownerForeignKey = entityEntry.Metadata.GetForeignKeys().First(fk => fk.IsOwnership);
            var ownerEntityType = ownerForeignKey.PrincipalEntityType.ClrType;

            shouldAuditOwnerEntity = IsTypeOfAuditedEntity(ownerEntityType);
            if (shouldAuditOwnerEntity.HasValue && !shouldAuditOwnerEntity.Value)
            {
                return false;
            }

            var ownerPropertyInfo = ownerForeignKey.PrincipalToDependent.PropertyInfo;
            shouldAuditOwnerProperty = IsAuditedPropertyInfo(ownerEntityType, ownerPropertyInfo);
            if (shouldAuditOwnerProperty.HasValue && !shouldAuditOwnerProperty.Value)
            {
                return false;
            }
        }

        return shouldAuditEntity ?? shouldAuditOwnerEntity ?? shouldAuditOwnerProperty ?? shouldTrackEntity;
    }

    protected virtual bool ShouldSavePropertyHistory(PropertyEntry propertyEntry, bool defaultValue)
    {
        var propertyInfo = propertyEntry.Metadata.PropertyInfo;
        if (propertyInfo == null) // Shadow properties or if mapped directly to a field
        {
            return defaultValue;
        }

        return IsAuditedPropertyInfo(propertyInfo) ?? defaultValue;
    }

    [CanBeNull]
    private EntityChange CreateEntityChange(EntityEntry entityEntry)
    {
        var entityId = GetEntityId(entityEntry);
        var entityTypeFullName = ProxyHelper.GetUnproxiedType(entityEntry.Entity).FullName;
        EntityChangeType changeType;
        switch (entityEntry.State)
        {
            case EntityState.Added:
                changeType = EntityChangeType.Created;
                break;
            case EntityState.Deleted:
                changeType = EntityChangeType.Deleted;
                break;
            case EntityState.Modified:
                changeType = entityEntry.IsDeleted() ? EntityChangeType.Deleted : EntityChangeType.Updated;
                break;
            case EntityState.Detached:
            case EntityState.Unchanged:
                return null;
            default:
                Logger.ErrorFormat("Unexpected {0} - {1}", nameof(entityEntry.State), entityEntry.State);
                return null;
        }

        if (entityId == null && changeType != EntityChangeType.Created)
        {
            Logger.ErrorFormat("EntityChangeType {0} must have non-empty entity id", changeType);
            return null;
        }

        return new EntityChange
        {
            ChangeType = changeType,
            EntityEntry = entityEntry, // [NotMapped]
            EntityId = entityId,
            EntityTypeFullName = entityTypeFullName,
            TenantId = AbpSession.TenantId
        };
    }

    /// <summary>
    /// Gets the property changes for this entry.
    /// </summary>
    private ICollection<EntityPropertyChange> GetPropertyChanges(EntityEntry entityEntry,
        bool auditedPropertiesOnly)
    {
        var propertyChanges = new List<EntityPropertyChange>();
        var properties = entityEntry.Metadata.GetProperties();

        foreach (var property in properties)
        {
            if (property.IsPrimaryKey())
            {
                continue;
            }

            var propertyEntry = entityEntry.Property(property.Name);

            if (ShouldSavePropertyHistory(propertyEntry, !auditedPropertiesOnly))
            {
                propertyChanges.Add(
                    CreateEntityPropertyChange(
                        propertyEntry.GetOriginalValue(),
                        propertyEntry.GetNewValue(),
                        property
                    )
                );
            }
        }

        return propertyChanges;
    }

    /// <summary>
    /// Updates change time, entity id, Adds foreign keys, Removes/Updates property changes after SaveChanges is called.
    /// </summary>
    private void UpdateChangeSet(EntityChangeSet changeSet)
    {
        var entityChangesToRemove = new List<EntityChange>();
        foreach (var entityChange in changeSet.EntityChanges)
        {
            var entityEntry = entityChange.EntityEntry.As<EntityEntry>();
            var entityEntryType = ProxyHelper.GetUnproxiedType(entityEntry.Entity);
            var isAuditedEntity = IsTypeOfAuditedEntity(entityEntryType) == true;

            /* Update change time */
            entityChange.ChangeTime = GetChangeTime(entityChange.ChangeType, entityEntry.Entity);

            /* Update entity id */
            entityChange.EntityId = GetEntityId(entityEntry);

            /* Update property changes */
            var trackedPropertyNames = entityChange.PropertyChanges.Select(pc => pc.PropertyName).ToList();

            var additionalForeignKeys = entityEntry.Metadata.GetDeclaredReferencingForeignKeys()
                                                .Where(fk => trackedPropertyNames.Contains(fk.Properties[0].Name))
                                                .ToList();

            /* Add additional foreign keys from navigation properties */
            foreach (var foreignKey in additionalForeignKeys)
            {
                foreach (var property in foreignKey.Properties)
                {
                    var shouldSaveProperty = property.PropertyInfo == null // Shadow properties or if mapped directly to a field
                        ? null
                        : IsAuditedPropertyInfo(entityEntryType, property.PropertyInfo);

                    if (shouldSaveProperty.HasValue && !shouldSaveProperty.Value)
                    {
                        continue;
                    }

                    var propertyEntry = entityEntry.Property(property.Name);

                    var newValue = propertyEntry.GetNewValue();
                    var oldValue = propertyEntry.GetOriginalValue();

                    // Add foreign key
                    entityChange.PropertyChanges.Add(CreateEntityPropertyChange(oldValue, newValue, property));
                }
            }

            /* Update/Remove property changes */
            var propertyChangesToRemove = new List<EntityPropertyChange>();
            var foreignKeys = entityEntry.Metadata.GetForeignKeys();
            foreach (var propertyChange in entityChange.PropertyChanges)
            {
                var propertyEntry = entityEntry.Property(propertyChange.PropertyName);

                // Take owner entity type if this is an owned entity
                var propertyEntityType = entityEntryType;
                if (entityEntry.Metadata.IsOwned())
                {
                    var ownerForeignKey = foreignKeys.First(fk => fk.IsOwnership);
                    propertyEntityType = ownerForeignKey.PrincipalEntityType.ClrType;
                }
                var property = propertyEntry.Metadata;
                var isAuditedProperty = property.PropertyInfo != null &&
                                        (IsAuditedPropertyInfo(propertyEntityType, property.PropertyInfo) ?? false);
                var isForeignKeyShadowProperty = property.IsShadowProperty() && foreignKeys.Any(fk => fk.Properties.Any(p => p.Name == propertyChange.PropertyName));

                propertyChange.SetNewValue(propertyEntry.GetNewValue()?.ToJsonString());
                if ((!isAuditedProperty && !isForeignKeyShadowProperty) || propertyChange.IsValuesEquals())
                {
                    // No change
                    propertyChangesToRemove.Add(propertyChange);
                }
            }

            foreach (var propertyChange in propertyChangesToRemove)
            {
                entityChange.PropertyChanges.Remove(propertyChange);
            }

            if (!isAuditedEntity && entityChange.PropertyChanges.Count == 0)
            {
                entityChangesToRemove.Add(entityChange);
            }
        }

        foreach (var entityChange in entityChangesToRemove)
        {
            changeSet.EntityChanges.Remove(entityChange);
        }
    }

    private EntityPropertyChange CreateEntityPropertyChange(object oldValue, object newValue, IProperty property)
    {
        var entityPropertyChange = new EntityPropertyChange()
        {
            PropertyName = property.Name.TruncateWithPostfix(EntityPropertyChange.MaxPropertyNameLength),
            PropertyTypeFullName = property.ClrType.FullName.TruncateWithPostfix(
                EntityPropertyChange.MaxPropertyTypeFullNameLength
            ),
            TenantId = AbpSession.TenantId
        };

        entityPropertyChange.SetNewValue(newValue?.ToJsonString());
        entityPropertyChange.SetOriginalValue(oldValue?.ToJsonString());
        return entityPropertyChange;
    }
}