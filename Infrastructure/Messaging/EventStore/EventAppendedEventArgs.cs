using AppliedSystems.Messaging.Messages;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;

    public class EventAppendedEventArgs : EventArgs
    {
        public string StreamName { get; private set; }
        public IEvent Event { get; private set; }

        public EventAppendedEventArgs(string streamName, IEvent @event)
        {
            StreamName = streamName;
            Event = @event;
        }
    }
}