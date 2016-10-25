namespace AppliedSystems.RiskCapture
{
    using System.Collections.Generic;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Polaris;
    using AppliedSystems.RiskCapture.Messages;

    public class ProductLineRiskCaptureMapState : AggregateState
    {
        public ProductLineCode ProductLine { get; }
        public Dictionary<string, ProductLineRiskCaptureSectionMap> Sections { get; }

        public ProductLineRiskCaptureMapState(AggregateId id, ProductLineCode productLine) : base(id)
        {
            ProductLine = productLine;
            Sections = new Dictionary<string, ProductLineRiskCaptureSectionMap>();
        }

        public void Apply(NewRiskSectionMapped @event)
        {
            if (@event.ProductLine != ProductLine)
            {
                return;
            }
            Sections[@event.SectionName] = new ProductLineRiskCaptureSectionMap(Id, @event.RiskSectionId, @event.ProductLine);
        }
    }
}