namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System;

    public class EventStoreUrl
    {
        private readonly Uri url;

        public static EventStoreUrl Parse(string @from)
        {
            return new EventStoreUrl(@from);
        }

        public static implicit operator Uri(EventStoreUrl from)
        {
            return @from.url;
        }

        private EventStoreUrl(string url)
        {
            this.url = new Uri(url);
        }

        public override string ToString()
        {
            return url.ToString();
        }
    }
}