namespace AppliedSystems.Infrastucture.Messaging.Http
{
    using AppliedSystems.Messaging.Infrastructure;
    using Core;

    public interface IIncomingMessageConverter
    {
        NotRequired<Message> Convert(string rawMessage);
    }
}