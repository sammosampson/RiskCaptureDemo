namespace AppliedSystems.RiskCapture
{
    using Infrastucture.Messaging.EventSourcing;

    public class ProductLineRiskCaptureSectionItemMap : AggregateEntity<RiskCaptureMap>
    {
        public ProductLineRiskCaptureSectionItemMap(RiskCaptureMap root) : base(root)
        {
        }
    }
}