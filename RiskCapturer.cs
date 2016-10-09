namespace AppliedSystems.RiskCapture
{
    using Infrastucture.Messaging.EventSourcing;
    using Messages;

    public class RiskCapturer : AggregateRoot
    {
        public void ProcessRequest(string request)
        {
            Then(new RiskCaptureProcessed(request));
        }

        public void Apply(RiskCaptureProcessed toApply)
        {
        }
    }
}