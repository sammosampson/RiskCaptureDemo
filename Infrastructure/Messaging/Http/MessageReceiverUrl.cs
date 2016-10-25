namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using System;

    public class MessageReceiverUrl
    {
        private readonly string url;

        private MessageReceiverUrl(string url)
        {
            this.url = url;
        }

        public static implicit operator string(MessageReceiverUrl from)
        {
            return from.url;
        }

        public static implicit operator Uri(MessageReceiverUrl from)
        {
            return new Uri(from.url);
        }

        public static MessageReceiverUrl Parse(string fromUrl)
        {
            return new MessageReceiverUrl(fromUrl);
        }

        public override string ToString()
        {
            return this.url;
        }
    }
}