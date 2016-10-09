using System;
using System.Collections;
using System.Collections.Generic;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public interface IReadEventStreamConnection : IDisposable
    {
        IReadEventStreamSubscription SubscribeToStream(string stream, int lastEventIndex);

        IEnumerable<Message> RetreiveAllEventsFromStream(string stream);

        void Close();
    }
}