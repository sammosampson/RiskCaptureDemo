using AppliedSystems.Messaging.Infrastructure.Events;
using AppliedSystems.Messaging.Infrastructure.Events.Outgoing;
using AppliedSystems.Messaging.Infrastructure.Pipelines;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public class EventStoreEventDispatchingEndpointBuilder : IEventDispatchingEndpointBuilder<EventStoreEventDispatchingEndpoint>
    {
        public IEventDispatcher Build(EventStoreEventDispatchingEndpoint endpoint, MessagePipelineBuilder pipelineBuilder)
        {
            return new EventStoreEventDispatcher(pipelineBuilder
                .AddPipelineComponent(new EventStreamStoragePipe())
                .Build());
        }
    }
}