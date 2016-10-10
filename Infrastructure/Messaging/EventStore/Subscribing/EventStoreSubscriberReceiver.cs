namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using System.Data;
    using System.Globalization;
    using System.Transactions;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Pipelines;
    using AppliedSystems.Messaging.Infrastructure.Receiving;
    using Core;
    using Data.Connections;

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

    public interface IEventIndexStore
    {
        void Store(string stream, int index);
    }

    public class SqlEventIndexStore : IEventIndexStore
    {
        private IConnectionFactory connectionFactory;

        public void Store(string stream, int index)
        {
            using (IDbConnection connection = connectionFactory.Create())
            {
                connection.Open();

                using (IDbCommand command = CreateCommand(connection, stream, index))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private IDbCommand CreateCommand(IDbConnection connection, string stream, int index)
        {
            IDbCommand command = connection.CreateCommand();

            command.CommandText = string.Format(
                CultureInfo.InvariantCulture,
                "IF NOT EXISTS (SELECT * FROM dbo.EventIndex) INSERT INTO dbo.EventIndex(Stream, Index) VALUES('{0}', {1}) ELSE UPDATE dbo.EventIndex SET Stream = '{0}', Index = {1}",
                stream,
                index);

            return command;
        }
    }
}