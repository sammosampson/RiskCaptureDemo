namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing;

    public class RiskCapture : AggregateRoot
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