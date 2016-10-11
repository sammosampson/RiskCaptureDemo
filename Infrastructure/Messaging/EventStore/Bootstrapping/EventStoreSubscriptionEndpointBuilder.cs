namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Subscribing;

    public class EventStoreSubscriptionEndpointBuilder : IReceivingEndpointBuilder<EventStoreSubscriptionEndpoint>
    {
        private readonly SubscribeEventStreamConnector connector;
        private readonly IEventIndexStore eventIndexStore;

        public EventStoreSubscriptionEndpointBuilder(SubscribeEventStreamConnector connector, IEventIndexStore eventIndexStore)
        {
            this.connector = connector;
            this.eventIndexStore = eventIndexStore;
        }

        public IEnumerable<IMessageReceiver> BuildReceivers(EventStoreSubscriptionEndpoint endpoint, MessagePipeline pipeline)
        {
            return new[] { new EventStoreSubscriberReceiver(pipeline, connector, endpoint.Credentials, endpoint.Url, eventIndexStore) };
        }

        public IEnumerable<ISubscriptionClient> BuildSubscriptionClients(EventStoreSubscriptionEndpoint endpoint)
        {
            return new[] { new EventStoreSubscritionClient(eventIndexStore) };
        }
    }
}