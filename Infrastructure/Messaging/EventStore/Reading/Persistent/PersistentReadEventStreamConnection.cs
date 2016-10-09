using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using AppliedSystems.Core;
using AppliedSystems.Core.Diagnostics;
using AppliedSystems.Messaging.Infrastructure;
using AppliedSystems.Messaging.Infrastructure.Headers;
using EventStore.ClientAPI;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading.Persistent
{
    public class PersistentReadEventStreamConnection : Disposable, IReadEventStreamConnection
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;
        private readonly IMessageDeliverer messageDeliverer;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentReadEventStreamConnection(IEventStoreConnection connection, IMessageDeliverer messageDeliverer, MessageStorageUserCredentials credentials)
        {
            this.connection = connection;
            this.messageDeliverer = messageDeliverer;
            this.credentials = credentials;
        }

        public IReadEventStreamSubscription SubscribeToStream(string stream, int lastEventIndex)
        {
            EventStoreStreamCatchUpSubscription subscription = connection.SubscribeToStreamFrom(stream, lastEventIndex, true, OnEventAppeared, OnLiveProcessingStarted, OnSubscriptionDropped, credentials);
            return new PersistentReadEventStreamSubscription(subscription);
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

        private void OnSubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason dropReason, Exception exception)
        {
            Trace.Information(
                "Subscription {0} dropped = {1}; {2}",
                subscription.StreamId,
                dropReason,
                exception?.Message ?? "No exception cause");
        }

        private void OnLiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
            Trace.Information("Live processing started {0}", subscription.StreamId);
        }

        private void OnEventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent @event)
        {
            Trace.Information("Event appeared on subscription {0}", subscription.StreamId);

            messageDeliverer.Deliver(subscription.StreamId, Message
                .Create(EventStoreJsonSerialiser
                    .DeserialiseFromJson(@event.Event.Data.ToUtf8()))
                .AddHeaders(EventStoreJsonSerialiser
                    .DeserialiseFromJson(@event.Event.Metadata.ToUtf8())
                    .As<Collection<MessageHeader>>()));
        }

        protected override void DisposeOfManagedResources()
        {
            connection.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}