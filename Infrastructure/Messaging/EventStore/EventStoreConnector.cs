namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;

    public class EventStoreConnector
    {
        private readonly MessageStorageUrl url;
        private readonly IEventStoreConnectionFactory eventStoreConnectionFactory;

        public EventStoreConnector(MessageStorageUrl url, IEventStoreConnectionFactory eventStoreConnectionFactory)
        {
            this.url = url;
            this.eventStoreConnectionFactory = eventStoreConnectionFactory;
        }

        public async Task<IEventStoreConnection> Connect()
        {
            var eventStoreConnection = eventStoreConnectionFactory.Create(url);
            await eventStoreConnection.ConnectAsync();
            return eventStoreConnection;
        }
    }
}