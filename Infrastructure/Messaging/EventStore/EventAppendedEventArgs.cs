namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using AppliedSystems.Messaging.Infrastructure;

    public class EventAppendedEventArgs : EventArgs
    {
        public string StreamName { get; private set; }
        public Message Event { get; private set; }

        public EventAppendedEventArgs(string streamName, Message @event)
        {
            StreamName = streamName;
            Event = @event;
        }
    }
}