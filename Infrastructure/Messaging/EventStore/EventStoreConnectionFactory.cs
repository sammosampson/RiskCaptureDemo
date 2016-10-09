namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using global::EventStore.ClientAPI;

    public class EventStoreConnectionFactory : IEventStoreConnectionFactory
    {
        public IEventStoreConnection Create(Uri url)
        {
            return EventStoreConnection.Create(url);
        }
    }
}