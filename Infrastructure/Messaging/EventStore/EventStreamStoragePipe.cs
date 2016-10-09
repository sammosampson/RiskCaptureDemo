using AppliedSystems.Core;
using AppliedSystems.Messaging.Infrastructure;
using AppliedSystems.Messaging.Infrastructure.Events.Streams;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    public class EventStreamStoragePipe : IMessagePipe
    {
        
        public NotRequired<Message> ProcessMessage(Message message)
        {
            WriteEventStreamConnectionContext.Current.AppendToStream(message.GetHeader(new EventStreamIdMessageHeaderKey(), s => s), message).Wait();
            return message;
        }
    }
}