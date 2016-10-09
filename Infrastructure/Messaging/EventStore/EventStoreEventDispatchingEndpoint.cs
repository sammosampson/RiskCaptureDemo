namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Events.Outgoing;

    public class EventStoreEventDispatchingEndpoint : IEventDispatchingEndpoint
    {
        public Type EndpointBuilderType => typeof(EventStoreEventDispatchingEndpointBuilder);
    }
}