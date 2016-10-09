namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class ProcessRiskCaptureRequest : ICommand
    {
        public string Request { get; }

        public ProcessRiskCaptureRequest(string request)
        {
            Request = request;
        }
    }
}