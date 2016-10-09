using System.Threading.Tasks;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing.Persistent
{
    public class PersistentWriteEventStreamConnector : IWriteEventStreamConnector
    {
        private readonly EventStoreConnector connector;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentWriteEventStreamConnector(EventStoreConnector connector, MessageStorageUserCredentials credentials)
        {
            this.connector = connector;
            this.credentials = credentials;
        }

        public async Task<IWriteEventStreamConnection> Connect()
        {
            var eventStoreConnection = await connector.Connect();
            return new PersistentWriteEventStreamConnection(eventStoreConnection, credentials);
        }
    }
}