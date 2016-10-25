namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Messages;

    public class DomainRepository
    {
        private readonly IEventStore store;

        public DomainRepository(IEventStore store)
        {
            this.store = store;
        }

        public TAggregateRoot Get<TAggregateRoot>(AggregateId aggregateRootId)
            where TAggregateRoot : IAggregateRoot, new()
        {
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.Rehydrate(aggregateRootId, GetEvents(aggregateRootId.ConvertToStreamName()));
            return aggregateRoot;
        }

        IEnumerable<IEvent> GetEvents(string aggregateRootId)
        {
            return store.GetEvents(aggregateRootId).Select(m => m.Payload.As<IEvent>()).ToList();
        }

        public IDisposable StartUnitOfWork()
        {
            var eventSourcingUnitOfWork = EventSourcingUnitOfWork.Start();
            eventSourcingUnitOfWork.OnEnding += EventSourcingUnitOfWork_OnEnding;
            return eventSourcingUnitOfWork;
        }

        private void EventSourcingUnitOfWork_OnEnding(object sender, EventArgs e)
        {
            store.StoreEvents(EventSourcingUnitOfWork.GetCurrent().GetEventsAdded());
            EventSourcingUnitOfWork.GetCurrent().OnEnding -= EventSourcingUnitOfWork_OnEnding;
        }
    }
}