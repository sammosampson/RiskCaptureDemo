namespace AppliedSystems.Infrastucture.Messaging.EventStore.Writing
{
    using System.Threading.Tasks;

    public class WriteEventStreamConnector 
    {
        private readonly EventStoreConnector connector;
        
        public WriteEventStreamConnector(EventStoreConnector connector)
        {
            this.connector = connector;
        }

        public async Task<WriteEventStreamConnection> Connect(EventStoreUrl url, EventStoreUserCredentials credentials)
        {
            var eventStoreConnection = await connector.Connect(url);
            return new WriteEventStreamConnection(eventStoreConnection, credentials);
        }
    }
}