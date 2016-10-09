using EventStore.ClientAPI;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading.Persistent
{
    public class PersistentReadEventStreamSubscription : IReadEventStreamSubscription
    {
        private readonly EventStoreStreamCatchUpSubscription inner;

        public PersistentReadEventStreamSubscription(EventStoreStreamCatchUpSubscription inner)
        {
            this.inner = inner;
        }

        public void Stop()
        {
            inner.Stop();
        }
    }
}