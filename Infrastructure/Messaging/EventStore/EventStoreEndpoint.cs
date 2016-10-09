namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System;

    public class EventStoreEndpoint : IEventStoreEndpoint
    {
        public static EventStoreEndpoint OnUrl(MessageStorageUrl url)
        {
            return new EventStoreEndpoint(url);
        }

        public EventStoreEndpoint WithCredentials(MessageStorageUserCredentials credentials)
        {
            Credentials = credentials;
            return this;
        }

        public Type EndpointBuilderType => typeof(EventStoreEndpointBuilder);

        public MessageStorageUrl Url { get; }

        public MessageStorageUserCredentials Credentials { get; private set; }

        private EventStoreEndpoint(MessageStorageUrl url)
        {
            Url = url;
        }
    }
}