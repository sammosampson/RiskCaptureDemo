namespace AppliedSystems.Infrastucture.Messaging.EventStore.Reading
{
    using System.Threading.Tasks;

    public class PersistentReadEventStreamConnector
    {
        private readonly EventStoreConnector connector;

        public PersistentReadEventStreamConnector(EventStoreConnector connector)
        {
            this.connector = connector;
        }

        public async Task<PersistentReadEventStreamConnection> Connect(MessageStorageUrl url)
        {
            var eventStoreConnection = await connector.Connect(url);
            return new PersistentReadEventStreamConnection(eventStoreConnection);
        }
    }
}