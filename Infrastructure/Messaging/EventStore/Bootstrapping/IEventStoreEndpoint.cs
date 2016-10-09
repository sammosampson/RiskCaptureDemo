namespace AppliedSystems.Infrastucture.Messaging.EventStore.Bootstrapping
{
    using System;

    public interface IEventStoreEndpoint
    {
        Type EndpointBuilderType { get; }
    }
}