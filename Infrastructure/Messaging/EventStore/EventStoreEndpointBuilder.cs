
namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System.Diagnostics;
    using Core.Diagnostics;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using EventSourcing;
    using Reading;
    using Writing;

    public class EventStoreEndpointBuilder : IEventStoreEndpointBuilder<EventStoreEndpoint>
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly PersistentWriteEventStreamConnector writeEventStreamConnector;
        private readonly PersistentReadEventStreamConnector readEventStreamConnector;

        public EventStoreEndpointBuilder(PersistentWriteEventStreamConnector writeEventStreamConnector, PersistentReadEventStreamConnector readEventStreamConnector)
        {
            this.writeEventStreamConnector = writeEventStreamConnector;
            this.readEventStreamConnector = readEventStreamConnector;
        }

        public IEventStore Build(EventStoreEndpoint endpoint, MessagePipelineBuilder pipelineBuilder)
        {           
            Trace.Information("Opening the event stream writer connection");

            PersistentWriteEventStreamConnection writeConnection = writeEventStreamConnector.Connect(endpoint.Url, endpoint.Credentials).Result;
            PersistentReadEventStreamConnection readConnection = readEventStreamConnector.Connect(endpoint.Url).Result;
        
            return new PersistentEventStore(pipelineBuilder, writeConnection, readConnection);
        }
    }
}