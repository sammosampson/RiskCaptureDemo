namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using AppliedSystems.Messaging.Infrastructure;

    public class NullMessageDeliverer : IMessageDeliverer
    {
        public void Deliver(string stream, Message toDeliver)
        {
        }
    }
}