namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System;
    using System.Threading.Tasks;
    using AppliedSystems.Messaging.Infrastructure;

    public class SubscribeEventStreamConnector
    {
        private readonly EventStoreConnector connector;

        public SubscribeEventStreamConnector(EventStoreConnector connector)
        {
            this.connector = connector;
        }

        public async Task<SubscribeEventStreamConnection> Connect(EventStoreUrl url, Action<Message> deliverer, EventStoreUserCredentials credentials)
        {
            var eventStoreConnection = await connector.Connect(url);
            return new SubscribeEventStreamConnection(eventStoreConnection, deliverer, credentials);
        }
    }
}