using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public interface IMessageDeliverer
    {
        void Deliver(string stream, Message toDeliver);
    }
}