namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.RiskCapture.Messages;

    public class ProductLineRiskCaptureSectionMapState : AggregateState
    {
        public Dictionary<string, ProductLineRiskCaptureSectionItemMap> Items { get; }
        public Guid RiskSectionId { get; }
        public string ProductLine { get; }

        public ProductLineRiskCaptureSectionMapState(AggregateId id, Guid riskSectionId, string productLine) : base(id)
        {
            RiskSectionId = riskSectionId;
            ProductLine = productLine;
            Items = new Dictionary<string, ProductLineRiskCaptureSectionItemMap>();

        }

        public void Apply(NewRiskItemMapped @event)
        {
            if (@event.ProductLine != ProductLine || @event.RiskSectionId != RiskSectionId)
            {
                return;
            }

            Items[@event.ItemName] = new ProductLineRiskCaptureSectionItemMap(Id, @event.ProductLine, @event.RiskSectionId, @event.RiskItemId);
        }
    }
}