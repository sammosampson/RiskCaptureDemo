namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Messages;

    public class DomainRepository
    {
        private readonly IEventRetreiver eventRetreiver;
        private readonly IEventDispatcher dispatcher;
        private readonly IEventStreamFactory eventStreamFactory;

        public DomainRepository(IEventRetreiver eventRetreiver, IEventDispatcher dispatcher, IEventStreamFactory eventStreamFactory)
        {
            this.eventRetreiver = eventRetreiver;
            this.dispatcher = dispatcher;
            this.eventStreamFactory = eventStreamFactory;
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
            using (eventStreamFactory.Create(aggregateRoot.GetEventStreamId()))
            {
                foreach (IEvent @event in aggregateRoot.EventsAdded)
                {
                    dispatcher.Raise(@event);
                }
            }
        }

        IEnumerable<IEvent> GetEvents(string aggregateRootId)
        {
            return eventRetreiver.GetEvents(aggregateRootId).ToList();
        }
    }
}