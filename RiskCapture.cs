namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;

    public class RiskCapture : AggregateRoot
    {
        public void ExtractCaptureFromRequest(RiskCaptureId id, string request, RiskCaptureMap map)
        {
            map.ExtractCaptureFromRequest(
                request, 
                (productLine, section, item, value) => Then(new RiskItemValueCaptured(id, productLine, section, item, value)));
        }
    }
}