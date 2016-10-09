namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;

    public interface IEventRetreiver : IDisposable
    {
        IEnumerable<IEvent> GetEvents(string streamId);
    }
}