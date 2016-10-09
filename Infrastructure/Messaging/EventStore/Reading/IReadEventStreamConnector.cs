using System.Threading.Tasks;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public interface IReadEventStreamConnector
    {
        Task<IReadEventStreamConnection> Connect();
    }
}