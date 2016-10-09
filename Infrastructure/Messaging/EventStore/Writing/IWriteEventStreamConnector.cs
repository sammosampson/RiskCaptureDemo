using System.Threading.Tasks;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Writing
{
    public interface IWriteEventStreamConnector
    {
        Task<IWriteEventStreamConnection> Connect();
    }
}