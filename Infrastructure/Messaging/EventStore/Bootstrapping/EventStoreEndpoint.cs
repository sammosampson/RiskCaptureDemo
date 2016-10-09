namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System;

    public class EventStoreEndpoint : IEventStoreEndpoint
    {
        public static EventStoreEndpoint OnUrl(EventStoreUrl url)
        {
            return new EventStoreEndpoint(url);
        }

        public EventStoreEndpoint WithCredentials(EventStoreUserCredentials credentials)
        {
            Credentials = credentials;
            return this;
        }

        public Type EndpointBuilderType => typeof(EventStoreEndpointBuilder);

        public EventStoreUrl Url { get; }

        public EventStoreUserCredentials Credentials { get; private set; }

        private EventStoreEndpoint(EventStoreUrl url)
        {
            Url = url;
        }
    }
}