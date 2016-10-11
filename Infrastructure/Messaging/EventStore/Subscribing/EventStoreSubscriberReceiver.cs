namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System.Transactions;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Core;

    public class EventStoreSubscriberReceiver : MessageReceiver
    {
        private readonly SubscribeEventStreamConnector connector;
        private readonly EventStoreUserCredentials credentials;
        private readonly EventStoreUrl url;
        private readonly IEventIndexStore eventIndexStore;

        public EventStoreSubscriberReceiver(
            MessagePipeline pipe,
            SubscribeEventStreamConnector connector,
            EventStoreUserCredentials credentials,
            EventStoreUrl url, 
            IEventIndexStore eventIndexStore) 
            : base(pipe)
        {
            this.connector = connector;
            this.credentials = credentials;
            this.url = url;
            this.eventIndexStore = eventIndexStore;
        }

        private void DeliverMessage(Message message)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                PutMessageInPipeline(message);

                eventIndexStore.Store(
                    message.GetHeader(new SubscrbedStreamIdMessageHeader(), s => s), 
                    message.GetHeader(new EventIndexMessageHeader(), s => s.ConvertToInt32()));

                scope.Complete();
            }
        }

        protected override void StartReceiving()
        {
            SubscribeEventStreamConnectionContext.CurrentConnection = connector.Connect(url, DeliverMessage, credentials).Result;
        }

        public override void StopReceiving()
        {
            SubscribeEventStreamConnectionContext.CurrentConnection.Close();
        }
    }
}