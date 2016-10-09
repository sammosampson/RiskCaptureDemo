namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class RiskCaptureProcessed : IEvent
    {
        public string Request { get; }

        public RiskCaptureProcessed(string request)
        {
            Request = request;
        }
    }
}