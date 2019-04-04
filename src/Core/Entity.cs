using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Entity
    {
        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        public IEnumerable<IDomainEvent> ConsumeDomainEvents()
        {
            var events = domainEvents.AsReadOnly();
            domainEvents.Clear();
            return events;
        }

        public bool HasDomainEvents => domainEvents.Any();
    }
}