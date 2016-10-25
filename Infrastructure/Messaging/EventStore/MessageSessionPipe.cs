namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Infrastucture.Messaging.EventStore.Writing;
    using AppliedSystems.Messaging.Infrastructure;
    using Core;

    public class MessageSessionPipe : IMessagePipe
    {
        private readonly WriteEventStreamConnection writeConnection;

        public MessageSessionPipe(WriteEventStreamConnection writeConnection)
        {
            this.writeConnection = writeConnection;
        }

        public NotRequired<Message> ProcessMessage(Message message)
        {
            writeConnection.AppendToStream(
                message.GetHeader(new EventSourcedAggregateIdMessageHeader(), s => s), 
                new [] { message }).Wait();

            return message;
        }

    }
}