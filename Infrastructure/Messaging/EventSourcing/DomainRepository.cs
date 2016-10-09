namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing
{
    using System.Collections.Generic;
    using System.Linq;
    using AppliedSystems.Messaging.Messages;

    public class DomainRepository
    {
        private readonly IEventStore store;

        public DomainRepository(IEventStore store)
        {
            this.store = store;
        }

        public bool Exists(string aggregateRootId)
        {
            return GetEvents(aggregateRootId).Any();
        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId)
            where TAggregateRoot : AggregateRoot, new()
        {
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.Rehydrate(aggregateRootId, GetEvents(aggregateRootId));
            return aggregateRoot;
        }

        public void Save<TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : AggregateRoot
        {
            store.StoreEvents(aggregateRoot.GetEventStreamId(), aggregateRoot.EventsAdded);
        }

        IEnumerable<IEvent> GetEvents(string aggregateRootId)
        {
            return store.GetEvents(aggregateRootId).ToList();
        }
    }
}