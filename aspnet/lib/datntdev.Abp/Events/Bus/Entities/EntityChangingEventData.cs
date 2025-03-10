using System;
using datntdev.Abp.Domain.Entities;

namespace datntdev.Abp.Events.Bus.Entities
{
    /// <summary>
    /// Used to pass data for an event when an entity (<see cref="IEntity"/>) is being changed (creating, updating or deleting).
    /// See <see cref="EntityCreatingEventData{TEntity}"/>, <see cref="EntityDeletingEventData{TEntity}"/> and <see cref="EntityUpdatingEventData{TEntity}"/> classes.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [Serializable]
    public class EntityChangingEventData<TEntity> : EntityEventData<TEntity>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="entity">Changing entity in this event</param>
        public EntityChangingEventData(TEntity entity)
            : base(entity)
        {

        }
    }
}