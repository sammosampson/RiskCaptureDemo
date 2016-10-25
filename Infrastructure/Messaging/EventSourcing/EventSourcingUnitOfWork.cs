namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Messages;

    public class EventSourcingUnitOfWork : Disposable
    {
        public event EventHandler<EventSourceEventArgs> EventReplayed;

        public event EventHandler<EventArgs> OnEnding;

        private static readonly ThreadLocal<EventSourcingUnitOfWork> Current = new ThreadLocal<EventSourcingUnitOfWork>();

        public static EventSourcingUnitOfWork Start()
        {
            return new EventSourcingUnitOfWork();
        }

        public static EventSourcingUnitOfWork GetCurrent()
        {
            return Current.Value;
        }

        private readonly List<Message> eventsAdded;

        private EventSourcingUnitOfWork()
        {
            Current.Value = this;
            eventsAdded = new List<Message>();
        }

        public void AddEvent(string aggregateRootId, IEvent toAdd)
        {
            ReplayEvent(toAdd);
            eventsAdded.Add(Message.Create(toAdd).AddHeader(new EventSourcedAggregateIdMessageHeader(), aggregateRootId));
        }

        public IEnumerable<Message> GetEventsAdded()
        {
            return eventsAdded;
        }

        public void ReplayEvent(IEvent toReplay)
        {
            EventReplayed?.Invoke(this, new EventSourceEventArgs(toReplay));
        }

        protected override void DisposeOfManagedResources()
        {
            OnEnding?.Invoke(this, EventArgs.Empty);
            Current.Value = null;
            base.DisposeOfManagedResources();
        }
    }
}