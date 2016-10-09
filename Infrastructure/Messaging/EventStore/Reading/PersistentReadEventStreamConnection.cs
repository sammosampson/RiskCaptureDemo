namespace AppliedSystems.Infrastucture.Messaging.EventStore.Reading
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using AppliedSystems.Messaging.Infrastructure;
    using Core;
    using Core.Diagnostics;
    using global::EventStore.ClientAPI;

    public class PersistentReadEventStreamConnection : Disposable
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;

        public PersistentReadEventStreamConnection(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Message> RetreiveAllEventsFromStream(string stream)
        {
            var messages = new List<Message>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = connection.ReadStreamEventsForwardAsync(stream, nextSliceStart, 200, false).Result;

                nextSliceStart = currentSlice.NextEventNumber;

                messages.AddRange(currentSlice.Events.Select(e => e.ToMessage()));
            }

            while (!currentSlice.IsEndOfStream);
            return messages;
        }

        public void Close()
        {
            connection.Close();
        }
        protected override void DisposeOfManagedResources()
        {
            connection.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}