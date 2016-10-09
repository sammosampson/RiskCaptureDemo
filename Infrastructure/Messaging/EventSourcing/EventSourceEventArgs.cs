namespace AppliedSystems.Infrastucture.Messaging.EventSourcing
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class EventSourceEventArgs : EventArgs
    {
        public IEvent Event { get; private set; }

        public EventSourceEventArgs(IEvent @event)
        {
            Event = @event;
        }
    }
}