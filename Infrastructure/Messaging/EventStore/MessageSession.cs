namespace AppliedSystems.Infrastucture.Messaging.EventStore
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using AppliedSystems.Messaging.Infrastructure;

    public class MessageSession : IEnumerable<Message>
    {
        private readonly ThreadLocal<List<Message>> inner;

        public MessageSession()
        {
            this.inner = new ThreadLocal<List<Message>>();
        }

        public void Add(Message message)
        {
            if (!inner.IsValueCreated)
            {
                inner.Value = new List<Message>();
            }
            inner.Value.Add(message);
        }

        public void Flush()
        {
            inner.Value.Clear();
        }

        public IEnumerator<Message> GetEnumerator()
        {
            if (!inner.IsValueCreated)
            {
                inner.Value = new List<Message>();
            }
            return inner.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}