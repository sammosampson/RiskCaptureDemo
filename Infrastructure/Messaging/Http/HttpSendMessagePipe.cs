namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Http;
    using AppliedSystems.Messaging.Http.Serialisation;
    using AppliedSystems.Messaging.Infrastructure;

    public class HttpSendMessagePipe : IMessagePipe
    {
        private const string JsonContentType = "application/json";
        private readonly MessagePublisherUrl uri;

        public HttpSendMessagePipe(MessagePublisherUrl uri)
        {
            this.uri = uri;
        }

        public NotRequired<Message> ProcessMessage(Message message)
        {
            new HttpPoster().ConfigureForUrl(uri).WithContentType(JsonContentType).WithTimeout(10000).Post(message.SerialiseFromJson());
            return message;
        }
    }
}