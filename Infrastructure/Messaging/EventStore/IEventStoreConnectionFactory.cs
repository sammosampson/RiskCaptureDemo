namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;
    using global::EventStore.ClientAPI;

    public interface IEventStoreConnectionFactory
    {
        IEventStoreConnection Create(Uri url);
    }
}