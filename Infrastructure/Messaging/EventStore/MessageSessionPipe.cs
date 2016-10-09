namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using AppliedSystems.Messaging.Infrastructure;
    using Core;

    public class MessageSessionPipe : IMessagePipe
    {
        private readonly MessageSession session;
        
        public MessageSessionPipe(MessageSession session)
        {
            this.session = session;
        }

        public NotRequired<Message> ProcessMessage(Message message)
        {
            session.Add(message);
            return message;
        }

    }
}