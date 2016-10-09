using EventStore.ClientAPI;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Subscribing
{
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