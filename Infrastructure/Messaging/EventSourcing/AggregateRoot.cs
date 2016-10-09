namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;
    using Collections;

    public abstract class AggregateRoot
    {
        readonly ConventionEventToHandlerRouter eventRouter;
        public EventHandler<EventSourceEventArgs> EventReplayed;
        string id;

        internal List<IEvent> EventsAdded { get; }

        internal string GetEventStreamId()
        {
            return id;
        }

        protected AggregateRoot()
        {
            EventsAdded = new List<IEvent>();
            eventRouter = new ConventionEventToHandlerRouter(this, "Apply");
        }

        protected AggregateRoot(string id) : this()
        {
            this.id = id;
        }

        protected void Then<TEvent>(TEvent @event) where TEvent : IEvent
        {
            ReplayEvent(@event);
            StoreEvent(@event);
        }

        void StoreEvent(IEvent @event)
        {
            EventsAdded.Add(@event);
        }

        internal void Rehydrate(string toRehaydrateId, IEnumerable<IEvent> events)
        {
            id = toRehaydrateId;

            events.ForEach(ReplayEvent);
        }

        void ReplayEvent(IEvent toReplay)
        {
            eventRouter.RouteEventToHandlers(toReplay);
            OnEventReplayed(toReplay);
        }

        void OnEventReplayed(IEvent @event)
        {
            EventReplayed?.Invoke(this, new EventSourceEventArgs(@event));
        }
    }
}