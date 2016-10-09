using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Subscribing
{
    public interface IMessageDeliverer
    {
        void Deliver(string stream, Message toDeliver);
    }
}