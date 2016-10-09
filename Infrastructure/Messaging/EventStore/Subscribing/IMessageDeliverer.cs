namespace AppliedSystems.Infrastucture.Messaging.EventStore.Subscribing
{
    using AppliedSystems.Messaging.Infrastructure;

    public interface IMessageDeliverer
    {
        void Deliver(string stream, Message toDeliver);
    }
}