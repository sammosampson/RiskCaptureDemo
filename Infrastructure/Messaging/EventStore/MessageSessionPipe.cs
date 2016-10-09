using System;
using AppliedSystems.Core;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
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