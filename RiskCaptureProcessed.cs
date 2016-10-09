namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.Messaging.Messages;

    public class RiskCaptureProcessed : IEvent
    {
        public string Request { get; }

        public RiskCaptureProcessed(string request)
        {
            Request = request;
        }
    }
}