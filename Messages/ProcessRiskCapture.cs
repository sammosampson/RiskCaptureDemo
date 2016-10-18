namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class ProcessRiskCapture : ICommand
    {
        public string Request { get; }

        public ProcessRiskCapture(string request)
        {
            Request = request;
        }
    }
}