namespace AppliedSystems.Infrastucture.Messaging.EventStore.Reading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AppliedSystems.Messaging.Infrastructure;
    using Core;
    using global::EventStore.ClientAPI;

    public class PersistentReadEventStreamConnection : Disposable
    {
        private readonly IEventStoreConnection connection;
        private const int ReadPageSize = 200;

        public PersistentReadEventStreamConnection(IEventStoreConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<Message> RetreiveAllEventsFromStream(string stream)
        {
            return RetreiveAllEventsFromStream(stream, re => re.ToMessage());
        }

        public IEnumerable<TEvent> RetreiveAllEventsFromStream<TEvent>(string stream, Func<ResolvedEvent, TEvent> converter)
        {
            var messages = new List<TEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = connection.ReadStreamEventsForwardAsync(stream, nextSliceStart, ReadPageSize, true).Result;

                nextSliceStart = currentSlice.NextEventNumber;

                messages.AddRange(currentSlice.Events.Select(converter));
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