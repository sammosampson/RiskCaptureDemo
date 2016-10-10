namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Headers;
    using Core;
    using Core.Diagnostics;
    using global::EventStore.ClientAPI;


    public class SubscribeEventStreamConnection : Disposable
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;
        private readonly Action<Message> messageDeliverer;
        private readonly EventStoreUserCredentials credentials;

        public SubscribeEventStreamConnection(IEventStoreConnection connection, Action<Message> messageDeliverer, EventStoreUserCredentials credentials)
        {
            this.connection = connection;
            this.messageDeliverer = messageDeliverer;
            this.credentials = credentials;
        }

        public EventStreamSubscription SubscribeToStream(string stream, int? lastEventIndex)
        {
            EventStoreStreamCatchUpSubscription subscription = connection.SubscribeToStreamFrom(
                stream, 
                lastEventIndex,
                CatchUpSubscriptionSettings.Default, 
                OnEventAppeared, 
                OnLiveProcessingStarted, 
                OnSubscriptionDropped, 
                credentials);

            return new EventStreamSubscription(subscription);
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

            messageDeliverer.Invoke(Message
                .Create(EventStoreJsonSerialiser.DeserialiseFromJson(@event.Event.Data.ToUtf8()))
                .AddHeaders(EventStoreJsonSerialiser.DeserialiseFromJson(@event.Event.Metadata.ToUtf8()).As<Collection<MessageHeader>>())
                .AddHeader(new EventIndexMessageHeader(), @event.OriginalEventNumber.ToString())
                .AddHeader(new SubscrbedStreamIdMessageHeader(), subscription.StreamId));
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