namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;

    public class EventStoreSubscriberReceiver : MessageReceiver
    {
        private readonly SubscribeEventStreamConnector connector;
        private readonly EventStoreUserCredentials credentials;
        private readonly EventStoreUrl url;
        
        public EventStoreSubscriberReceiver(
            MessagePipeline pipe,
            SubscribeEventStreamConnector connector,
            EventStoreUserCredentials credentials,
            EventStoreUrl url) 
            : base(pipe)
        {
            this.connector = connector;
            this.credentials = credentials;
            this.url = url;
        }

        private void DeliverMessage(Message message)
        {
            PutMessageInPipeline(message);
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