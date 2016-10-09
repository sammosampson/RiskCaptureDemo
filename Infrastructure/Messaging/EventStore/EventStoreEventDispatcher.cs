namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Messages;

    public class EventStoreEventDispatcher : IEventDispatcher
    {
        private readonly MessagePipeline pipeline;

        public EventStoreEventDispatcher(MessagePipeline pipeline)
        {
            this.pipeline = pipeline;
        }

        public void Raise<TEvent>(TEvent message, Guid? id = null, Guid? correlationId = null) 
            where TEvent : IEvent
        {
            pipeline.ReceiveMessage(Message.Create(message, id, correlationId));
        }
    }
}