namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Subscribing;

    public class EventStoreSubscritionClient : ISubscriptionClient
    {
        private readonly Dictionary<string, EventStreamSubscription> subscriptions;

        public EventStoreSubscritionClient()
        {
            subscriptions = new Dictionary<string, EventStreamSubscription>();
        }

        public void Subscribe(string streamName)
        {
            subscriptions[streamName] = SubscribeEventStreamConnectionContext.CurrentConnection.SubscribeToStream(streamName, null);
        }

        public void Unsubscribe(string streamName)
        {
            subscriptions[streamName].Stop();
        }
    }
}