namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Messages;
    using EventSourcing;

    public class NonPersistentEventStore : IEventStore
    {
        public event EventHandler<EventAppendedEventArgs> EventAppended;
        
        private void OnEventAppended(string streamId, IEvent toStore)
        {
            EventAppended?.Invoke(this, new EventAppendedEventArgs(streamId, toStore));
        }

        public IEnumerable<Message> GetEvents(string streamId)
        {
            throw new NotImplementedException();
        }

        public void StoreEvents(IEnumerable<Message> toStore)
        {
            throw new NotImplementedException();
        }
    }
}