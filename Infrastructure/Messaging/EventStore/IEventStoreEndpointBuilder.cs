namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using EventSourcing;

    public interface IEventStoreEndpointBuilder<in TEventStoreEndpoint> where TEventStoreEndpoint : IEventStoreEndpoint
    {
        IEventStore Build(TEventStoreEndpoint endpoint, MessagePipelineBuilder pipelineBuilder);
    }
}