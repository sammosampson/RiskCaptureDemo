namespace AppliedSystems.RiskCapture
{
    using Core;
    using Infrastucture.Messaging.Http;
    using Messages;
    using Messaging.Infrastructure;

    public class IncomingMessageConverter : IIncomingMessageConverter
    {
        public NotRequired<Message> Convert(string rawMessage)
        {
            return Message.Create(new ProcessRiskCaptureRequest(rawMessage));
        }
    }
}