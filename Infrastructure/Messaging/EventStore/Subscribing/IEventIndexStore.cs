namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    public interface IEventIndexStore
    {
        void Store(string stream, int index);

        int? Get(string streamName);
    }
}