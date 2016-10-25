
namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System.Diagnostics;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using Core.Diagnostics;
    using EventSourcing;
    using Reading;
    using Writing;

    public class EventStoreEndpointBuilder : IEventStoreEndpointBuilder<EventStoreEndpoint>
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly WriteEventStreamConnector writeEventStreamConnector;
        private readonly PersistentReadEventStreamConnector readEventStreamConnector;

        public EventStoreEndpointBuilder(
            WriteEventStreamConnector writeEventStreamConnector, 
            PersistentReadEventStreamConnector readEventStreamConnector)
        {
            this.writeEventStreamConnector = writeEventStreamConnector;
            this.readEventStreamConnector = readEventStreamConnector;
        }

        public IEventStore BuildEventStore(EventStoreEndpoint endpoint, MessagePipelineBuilder pipelineBuilder)
        {           
            Trace.Information("Opening the event stream writer connection");

            WriteEventStreamConnection writeConnection = writeEventStreamConnector.Connect(endpoint.Url, endpoint.Credentials).Result;
            PersistentReadEventStreamConnection readConnection = readEventStreamConnector.Connect(endpoint.Url).Result;
        
            return new EventStore(
                readConnection, 
                pipelineBuilder.AddPipelineComponent(new MessageSessionPipe(writeConnection)).Build());
        }

        public IProjectionStore BuildProjectionStore(EventStoreEndpoint endpoint)
        {
            PersistentReadEventStreamConnection readConnection = readEventStreamConnector.Connect(endpoint.Url).Result;
            return new ProjectionStore(readConnection);
        }
    }
}