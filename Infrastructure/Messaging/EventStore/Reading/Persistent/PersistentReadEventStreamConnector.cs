using System.Threading.Tasks;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading.Persistent
{
    public class PersistentReadEventStreamConnector : IReadEventStreamConnector
    {
        private readonly EventStoreConnector connector;
        private readonly IMessageDeliverer messageDeliverer;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentReadEventStreamConnector(EventStoreConnector connector, IMessageDeliverer messageDeliverer, MessageStorageUserCredentials credentials)
        {
            this.connector = connector;
            this.messageDeliverer = messageDeliverer;
            this.credentials = credentials;
        }

        public async Task<IReadEventStreamConnection> Connect()
        {
            var eventStoreConnection = await connector.Connect();
            return new PersistentReadEventStreamConnection(eventStoreConnection, messageDeliverer, credentials);
        }
    }
}