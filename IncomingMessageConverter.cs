namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.Core;
    using AppliedSystems.Messaging.Infrastructure;
    using Infrastucture.Messaging.Http;

    public class IncomingMessageConverter : IIncomingMessageConverter
    {
        public NotRequired<Message> Convert(string rawMessage)
        {
            return Message.Create(new ProcessRiskCaptureRequest(rawMessage));
        }
    }
}