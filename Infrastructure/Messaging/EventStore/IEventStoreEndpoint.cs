namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System;

    public interface IEventStoreEndpoint
    {
        Type EndpointBuilderType { get; }
    }
}