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

    public class PersistentSubscribeEventStreamConnection : Disposable
    {
        private static readonly TraceSource Trace = TraceSourceProvider.Provide();

        private readonly IEventStoreConnection connection;
        private readonly IMessageDeliverer messageDeliverer;
        private readonly MessageStorageUserCredentials credentials;

        public PersistentSubscribeEventStreamConnection(IEventStoreConnection connection, IMessageDeliverer messageDeliverer, MessageStorageUserCredentials credentials)
        {
            this.connection = connection;
            this.messageDeliverer = messageDeliverer;
            this.credentials = credentials;
        }

        public PersistentEventStreamSubscription SubscribeToStream(string stream, int lastEventIndex)
        {
            EventStoreStreamCatchUpSubscription subscription = connection.SubscribeToStreamFrom(
                stream, 
                lastEventIndex, 
                true, 
                OnEventAppeared, 
                OnLiveProcessingStarted, 
                OnSubscriptionDropped, 
                credentials);

            return new PersistentEventStreamSubscription(subscription);
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
            connection.Close();
            connection.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}