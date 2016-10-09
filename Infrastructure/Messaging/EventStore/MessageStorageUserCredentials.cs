namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using global::EventStore.ClientAPI.SystemData;

    public class MessageStorageUserCredentials
    {
        public static implicit operator UserCredentials(MessageStorageUserCredentials from)
        {
            return from.inner;
        }

        public static MessageStorageUserCredentials Parse(string user, string password)
        {
            return new MessageStorageUserCredentials(user, password);
        }

        readonly UserCredentials inner;

        private MessageStorageUserCredentials(string user, string password)
        {
            inner = new UserCredentials(user, password);
        }
    }
}