namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Collections.Generic;
    using System.Linq;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Messages;
    using Collections;
    using Core;
    using EventSourcing;
    using Reading;
    using Writing;

    public class EventStore : IEventStore
    {
        private readonly WriteEventStreamConnection writeConnection;
        private readonly PersistentReadEventStreamConnection readConnection;
        private readonly MessageSession currentMessageSession;
        private readonly MessagePipeline pipe;

        public EventStore(MessagePipelineBuilder messagePipelineBuilder, WriteEventStreamConnection writeConnection, PersistentReadEventStreamConnection readConnection)
        {
            this.writeConnection = writeConnection;
            this.readConnection = readConnection;

            currentMessageSession = new MessageSession();

            pipe = messagePipelineBuilder
                .AddPipelineComponent(new MessageSessionPipe(currentMessageSession))
                .Build();
        }

        public IEnumerable<IEvent> GetEvents(string streamId)
        {
            return readConnection.RetreiveAllEventsFromStream(streamId).Select(m => m.Payload.As<IEvent>());
        }

        public void StoreEvents(string streamId, IEnumerable<IEvent> toStore)
        {
            toStore.ForEach(e => pipe.ReceiveMessage(Message.Create(e)));
            writeConnection.AppendToStream(streamId, currentMessageSession).Wait();
            currentMessageSession.Flush();
        }
    }
}