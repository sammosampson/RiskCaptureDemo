using System.Threading.Tasks;
using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Subscribing;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
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