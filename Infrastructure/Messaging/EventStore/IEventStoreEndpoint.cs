using System;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public interface IEventStoreEndpoint
    {
        Type EndpointBuilderType { get; }
    }
}