using System;
using System.Collections.Generic;
using AppliedSystems.Messaging.Messages;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetEvents(string streamId);

        void StoreEvents(string streamId, IEnumerable<IEvent> toStore);
    }
}