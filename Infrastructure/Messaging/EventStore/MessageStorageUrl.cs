namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore
{
    using System;

    public class MessageStorageUrl
    {
        private readonly Uri url;

        public static MessageStorageUrl Parse(string @from)
        {
            return new MessageStorageUrl(@from);
        }

        public static implicit operator Uri(MessageStorageUrl from)
        {
            return @from.url;
        }

        private MessageStorageUrl(string url)
        {
            this.url = new Uri(url);
        }

        public override string ToString()
        {
            return url.ToString();
        }
    }
}