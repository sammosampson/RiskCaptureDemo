using System.Threading.Tasks;
using AppliedSystems.Core;
using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing.NonPersistent
{
    public class NonPersistentWriteEventStreamConnection : Disposable, IWriteEventStreamConnection
    {
        private readonly NonPersistentEventStore eventStore;

        public NonPersistentWriteEventStreamConnection(NonPersistentEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public Task AppendToStream(string streamName, Message toStore)
        {
            eventStore.AppendToStream(streamName, toStore);
            return Task.FromResult(false);
        }

        public void Close()
        {
        }
    }
}