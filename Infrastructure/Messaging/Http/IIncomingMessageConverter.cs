namespace AppliedSystems.RiskCapture.Infrastucture.Messaging.Http
{
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;

    public interface IIncomingMessageConverter
    {
        NotRequired<Message> Convert(string rawMessage);
    }
}