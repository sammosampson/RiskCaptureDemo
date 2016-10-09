namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using EventSourcing;

    public interface IEventStoreEndpointBuilder<in TEventStoreEndpoint> where TEventStoreEndpoint : IEventStoreEndpoint
    {
        IEventStore Build(TEventStoreEndpoint endpoint, MessagePipelineBuilder pipelineBuilder);
    }
}