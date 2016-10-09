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

        public EventStoreSubscriptionEndpointBuilder(SubscribeEventStreamConnector connector)
        {
            this.connector = connector;
        }

        public IEnumerable<IMessageReceiver> BuildReceivers(EventStoreSubscriptionEndpoint endpoint, MessagePipeline pipeline)
        {
            return new[] { new EventStoreSubscriberReceiver(pipeline, connector, endpoint.Credentials, endpoint.Url) };
        }

        public IEnumerable<ISubscriptionClient> BuildSubscriptionClients(EventStoreSubscriptionEndpoint endpoint)
        {
            return new[] { new EventStoreSubscritionClient() };
        }
    }
}