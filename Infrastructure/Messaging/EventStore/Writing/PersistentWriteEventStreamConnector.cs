namespace AppliedSystems.Infrastucture.Messaging.EventStore.Writing
{
    using System.Threading.Tasks;

    public class PersistentWriteEventStreamConnector 
    {
        private readonly EventStoreConnector connector;
        
        public PersistentWriteEventStreamConnector(EventStoreConnector connector)
        {
            this.connector = connector;
        }

        public async Task<PersistentWriteEventStreamConnection> Connect(MessageStorageUrl url, MessageStorageUserCredentials credentials)
        {
            var eventStoreConnection = await connector.Connect(url);
            return new PersistentWriteEventStreamConnection(eventStoreConnection, credentials);
        }
    }
}