using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Subscribing
{
    public class NullMessageDeliverer : IMessageDeliverer
    {
        public void Deliver(string stream, Message toDeliver)
        {
        }
    }
}