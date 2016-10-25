namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Collections.Generic;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Reading;

    public class ProjectionStore : IProjectionStore
    {
        private readonly PersistentReadEventStreamConnection readConnection;

        public ProjectionStore(PersistentReadEventStreamConnection readConnection)
        {
            this.readConnection = readConnection;
        }

        public IEnumerable<TProjectionItem> GetProjection<TProjectionItem>(string streamId)
        {
            return readConnection.RetreiveAllEventsFromStream(streamId, re => re.ToProjectionItem<TProjectionItem>());
        }
    }
}