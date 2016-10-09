using System;
using System.Collections.Generic;
using AppliedSystems.Collections;
using AppliedSystems.Messaging.Messages;
using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public class NonPersistentEventStore : IEventStore
    {
        public event EventHandler<EventAppendedEventArgs> EventAppended;
        
        private void OnEventAppended(string streamId, IEvent toStore)
        {
            EventAppended?.Invoke(this, new EventAppendedEventArgs(streamId, toStore));
        }

        public IEnumerable<IEvent> GetEvents(string streamId)
        {
            throw new NotImplementedException();
        }

        public void StoreEvents(string streamId, IEnumerable<IEvent> toStore)
        {
            toStore.ForEach(message => OnEventAppended(streamId, message));
        }
    }
}