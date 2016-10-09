namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using global::EventStore.ClientAPI.SystemData;

    public class EventStoreUserCredentials
    {
        public static implicit operator UserCredentials(EventStoreUserCredentials from)
        {
            return from.inner;
        }

        public static EventStoreUserCredentials Parse(string user, string password)
        {
            return new EventStoreUserCredentials(user, password);
        }

        readonly UserCredentials inner;

        private EventStoreUserCredentials(string user, string password)
        {
            inner = new UserCredentials(user, password);
        }
    }
}