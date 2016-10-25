namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.RiskCapture.Messages;

    public class RiskCapture : AggregateRoot<RiskCaptureState>
    {
        public RiskCapture() : base(new RiskCaptureState())
        {
        }

        public void ExtractCaptureFromRequest(RiskCaptureId id, string request, RiskCaptureMap map)
        {
            map.ExtractCaptureFromRequest(
                request, 
                (productLine, section, item, value) => Then(new RiskItemValueCaptured(id, productLine, section, item, value)));
        }
    }
}