using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AppliedSystems.CodeAnalysis;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing.NonPersistent
{
    public class NonPersistentWriteEventStreamConnector : IWriteEventStreamConnector
    {
        private readonly NonPersistentEventStore store;

        public NonPersistentWriteEventStreamConnector(NonPersistentEventStore store)
        {
            this.store = store;
        }

        [SuppressMessage(FxCop.Reliability, FxCop.CA2000, Justification = Justifications.CA2000FactoryMethod)]
        public Task<IWriteEventStreamConnection> Connect()
        {
            IWriteEventStreamConnection connection = new NonPersistentWriteEventStreamConnection(store);
            return Task.FromResult(connection);
        }
    }
}