namespace AppliedSystems.RiskCapture
{
    using System.Collections.Generic;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Polaris;
    using AppliedSystems.RiskCapture.Messages;

    public class RiskCaptureMapState : AggregateState
    {
        public Dictionary<ProductLineCode, ProductLineRiskCaptureMap> ProductLines { get; }

        public RiskCaptureMapState()
        {
            ProductLines = new Dictionary<ProductLineCode, ProductLineRiskCaptureMap>();
        }

        public void Apply(NewRiskProductLineMapped @event)
        {
            var productLine = ProductLineCode.Parse(@event.ProductLine);
            ProductLines[productLine] = new ProductLineRiskCaptureMap(Id, productLine);
        }
    }
}