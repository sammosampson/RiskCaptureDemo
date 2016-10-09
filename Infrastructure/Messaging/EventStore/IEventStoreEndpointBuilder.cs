using AppliedSystems.Messaging.Infrastructure.Pipelines;
using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public interface IEventStoreEndpointBuilder<in TEventStoreEndpoint> where TEventStoreEndpoint : IEventStoreEndpoint
    {
        IEventStore Build(TEventStoreEndpoint endpoint, MessagePipelineBuilder pipelineBuilder);
    }
}