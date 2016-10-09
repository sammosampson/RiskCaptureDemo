namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;

    public class EventStoreConnector
    {
        public async Task<IEventStoreConnection> Connect(EventStoreUrl url)
        {
            var eventStoreConnection = EventStoreConnection.Create(url);
            await eventStoreConnection.ConnectAsync();
            return eventStoreConnection;
        }
    }
}