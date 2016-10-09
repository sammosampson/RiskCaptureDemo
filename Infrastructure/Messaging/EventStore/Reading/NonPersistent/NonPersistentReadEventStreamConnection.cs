using System.Collections.Generic;
using AppliedSystems.Core;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading.NonPersistent
{
    public class NonPersistentReadEventStreamConnection : Disposable, IReadEventStreamConnection
    {
        public IReadEventStreamSubscription SubscribeToStream(string stream, int lastEventIndex)
        {
            return new NonPersistentReadEventStreamSubscription();
        }

        public IEnumerable<Message> RetreiveAllEventsFromStream(string stream)
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
        }
    }
}