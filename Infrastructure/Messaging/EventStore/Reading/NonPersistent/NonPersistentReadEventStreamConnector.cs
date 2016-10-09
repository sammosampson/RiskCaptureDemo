using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AppliedSystems.CodeAnalysis;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading.NonPersistent
{
    public class NonPersistentReadEventStreamConnector : IReadEventStreamConnector
    {
        [SuppressMessage(FxCop.Reliability, FxCop.CA2000, Justification = Justifications.CA2000FactoryMethod)]
        public Task<IReadEventStreamConnection> Connect()
        {
            IReadEventStreamConnection connection = new NonPersistentReadEventStreamConnection();
            return Task.FromResult(connection);
        }
    }
}