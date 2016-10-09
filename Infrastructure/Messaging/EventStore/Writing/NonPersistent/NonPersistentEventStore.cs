using System;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing.NonPersistent
{
    public class NonPersistentEventStore
    {
        public event EventHandler<EventAppendedEventArgs> EventAppended;

        public void AppendToStream(string streamName, Message toStore)
        {
            OnEventAppended(streamName, toStore);
        }

        private void OnEventAppended(string streamName, Message toStore)
        {
            EventAppended?.Invoke(this, new EventAppendedEventArgs(streamName, toStore));
        }
    }
}