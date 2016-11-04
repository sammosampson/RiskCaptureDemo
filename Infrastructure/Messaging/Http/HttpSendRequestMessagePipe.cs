namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Http.Serialisation;
    using AppliedSystems.Messaging.Infrastructure;
    using AppliedSystems.Messaging.Infrastructure.Requests;
    using AppliedSystems.Messaging.Messages;

    public class HttpSendRequestMessagePipe : IMessagePipe
    {
        private const string JsonContentType = "application/json";
        private readonly MessageReceiverUrl uri;

        public HttpSendRequestMessagePipe(MessageReceiverUrl uri)
        {
            this.uri = uri;
        }

        public NotRequired<Message> ProcessMessage(Message message)
        {
            string rawResponse = new HttpPoster().ConfigureForUrl(uri).WithContentType(JsonContentType).WithTimeout(10000).Post(message.SerialiseFromJson());
            ReplyContext.CallCurrentResponseHandler(rawResponse.DeserialiseFromJson().Payload.As<IResponse>());
            return message;
        }
    }
}