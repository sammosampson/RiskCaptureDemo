using System;
using System.Linq;
using AppliedSystems.Core;
using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.EventSourcing
{
    using System.Collections.Generic;
    using AppliedSystems.Messaging.Messages;
    using Messaging.EventSourcing;

    public class EventStoreEventRetreiver : IEventRetreiver
    {
        public void Dispose()
        {
        }

        public IEnumerable<IEvent> GetEvents(string streamId)
        {
            return ReadEventStreamConnectionContext.Current
                .RetreiveAllEventsFromStream(streamId)
                .Select(m => m.Payload.As<IEvent>());
        }
    }

    public class EventStoreEventStreamFactory : IEventStreamFactory
    {
        public IDisposable Create(string eventStream)
        {
            return new EventStoreEventStream(eventStream);
        }
    }

    public class EventStoreEventStream : Disposable
    {
        private readonly string eventStream;

        public EventStoreEventStream(string eventStream)
        {
            _eventStream = eventStream;
        }

        protected override void DisposeOfManagedResources()
        {
            WriteEventStreamConnectionContext.Current.AppendToStream()
        }
    }
}