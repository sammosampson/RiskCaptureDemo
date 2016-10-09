using System;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing
{
    public interface IEventStreamFactory
    {
        IDisposable Create(string eventStream);
    }
}