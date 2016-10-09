namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using global::EventStore.ClientAPI;

    public class EventStreamSubscription
    {
        private readonly EventStoreStreamCatchUpSubscription inner;

        public EventStreamSubscription(EventStoreStreamCatchUpSubscription inner)
        {
            this.inner = inner;
        }

        public void Stop()
        {
            inner.Stop();
        }
    }
}