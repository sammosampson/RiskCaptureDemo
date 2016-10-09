namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.Messaging.Messages;

    public class ProcessRiskCaptureRequest : ICommand
    {
        public string Request { get; }

        public ProcessRiskCaptureRequest(string request)
        {
            Request = request;
        }
    }
}