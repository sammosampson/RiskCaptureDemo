namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public class EventStoreSubscriptionEndpoint : IReceivingEndpoint
    {
        public static EventStoreSubscriptionEndpoint ListenTo(EventStoreUrl url)
        {
            return new EventStoreSubscriptionEndpoint(url);
        }

        public EventStoreSubscriptionEndpoint WithCredentials(EventStoreUserCredentials credentials)
        {
            Credentials = credentials;
            return this;
        }

        public Type EndpointBuilderType => typeof(EventStoreSubscriptionEndpointBuilder);

        public EventStoreUserCredentials Credentials { get; private set; }

        public EventStoreUrl Url { get; private set; }

        private EventStoreSubscriptionEndpoint(EventStoreUrl url)
        {
            Url = url;
        }
    }
}