namespace AppliedSystems.RiskCapture.Mapping
{
    using System.Collections.Generic;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Polaris;
    using AppliedSystems.RiskCapture.Messages;

    public class MapState : AggregateState
    {
        public Dictionary<ProductLineCode, ProductLine> ProductLines { get; }

        public MapState()
        {
            ProductLines = new Dictionary<ProductLineCode, ProductLine>();
        }

        public void Apply(NewRiskProductLineMapped @event)
        {
            var productLine = ProductLineCode.Parse(@event.ProductLine);
            ProductLines[productLine] = new ProductLine(Id, productLine);
        }
    }
}