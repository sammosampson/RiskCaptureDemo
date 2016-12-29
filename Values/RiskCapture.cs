namespace AppliedSystems.RiskCapture.Values
{
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.RiskCapture.Mapping;
    using AppliedSystems.RiskCapture.Messages;

    public class RiskCapture : AggregateRoot<RiskCaptureState>
    {
        public RiskCapture() : base(new RiskCaptureState())
        {
        }

        public void ExtractCaptureFromRequest(RiskCaptureId id, string request, Map map)
        {
            map.ExtractCaptureFromRequest(
                request, 
                (productLine, section, item, value) => Then(new RiskItemValueCaptured(id, productLine, section, item, value)));
        }
    }
}