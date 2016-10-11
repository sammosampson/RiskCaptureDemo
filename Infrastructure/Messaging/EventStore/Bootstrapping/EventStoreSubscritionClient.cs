namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Infrastructure.Events;
    using Subscribing;

    public class EventStoreSubscritionClient : ISubscriptionClient
    {
        private readonly Dictionary<string, EventStreamSubscription> subscriptions;
        private readonly IEventIndexStore store;

        public EventStoreSubscritionClient(IEventIndexStore store)
        {
            this.store = store;
            subscriptions = new Dictionary<string, EventStreamSubscription>();
        }

        public void Subscribe(string streamName)
        {
            subscriptions[streamName] = SubscribeEventStreamConnectionContext.CurrentConnection.SubscribeToStream(streamName, GetLastEventIndex(streamName));
        }

        private int? GetLastEventIndex(string streamName)
        {
            return store.Get(streamName);
        }

        public void Unsubscribe(string streamName)
        {
            subscriptions[streamName].Stop();
        }
    }
}