using AppliedSystems.Messaging.Infrastructure;

namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.EventStore.Reading
{
    public class NullMessageDeliverer : IMessageDeliverer
    {
        public void Deliver(string stream, Message toDeliver)
        {
        }
    }
}