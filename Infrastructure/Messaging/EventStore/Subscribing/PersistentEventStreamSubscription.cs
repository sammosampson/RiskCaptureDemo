namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using global::EventStore.ClientAPI;

    public class PersistentEventStreamSubscription
    {
        private readonly EventStoreStreamCatchUpSubscription inner;

        public PersistentEventStreamSubscription(EventStoreStreamCatchUpSubscription inner)
        {
            this.inner = inner;
        }

        public void Stop()
        {
            inner.Stop();
        }
    }
}