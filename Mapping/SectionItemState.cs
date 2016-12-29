namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using AppliedSystems.Domain.EventSourced;

    public class SectionItemState : AggregateState
    {
        public string ProductLine { get; }
        public Guid RiskSectionId { get; }
        public Guid RiskItemId { get; }

        public SectionItemState(AggregateId id, string productLine, Guid riskSectionId, Guid riskItemId) 
            : base(id)
        {
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            RiskItemId = riskItemId;
        }
    }
}