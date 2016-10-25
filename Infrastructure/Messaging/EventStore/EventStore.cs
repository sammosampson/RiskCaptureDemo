namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using Collections;
    using EventSourcing;
    using Reading;

    public class EventStore : IEventStore
    {
        private readonly PersistentReadEventStreamConnection readConnection;
        private readonly MessagePipeline pipe;

        public EventStore(PersistentReadEventStreamConnection readConnection, MessagePipeline pipe)
        {
            this.readConnection = readConnection;
            this.pipe = pipe;
        }

        public IEnumerable<Message> GetEvents(string streamId)
        {
            return readConnection.RetreiveAllEventsFromStream(streamId);
        }

        public void StoreEvents(IEnumerable<Message> toStore)
        {
            toStore.ForEach(e => pipe.ReceiveMessage(e));
        }
    }
}