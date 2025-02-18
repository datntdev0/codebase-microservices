using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using datntdev.Abp.Events.Bus;

namespace datntdev.Abp.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {

    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        [NotMapped]
        public virtual ICollection<IEventData> DomainEvents { get; }

        public AggregateRoot()
        {
            DomainEvents = new Collection<IEventData>();
        }
    }
}