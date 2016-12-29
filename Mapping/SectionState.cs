namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.RiskCapture.Messages;

    public class SectionState : AggregateState
    {
        public Dictionary<string, SectionItem> Items { get; }
        public Guid RiskSectionId { get; }
        public string ProductLine { get; }

        public SectionState(AggregateId id, Guid riskSectionId, string productLine) : base(id)
        {
            RiskSectionId = riskSectionId;
            ProductLine = productLine;
            Items = new Dictionary<string, SectionItem>();

        }

        public void Apply(NewRiskItemMapped @event)
        {
            if (@event.ProductLine != ProductLine || @event.RiskSectionId != RiskSectionId)
            {
                return;
            }

            Items[@event.ItemName] = new SectionItem(Id, @event.ProductLine, @event.RiskSectionId, @event.RiskItemId);
        }
    }
}