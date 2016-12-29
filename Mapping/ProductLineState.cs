namespace AppliedSystems.RiskCapture.Mapping
{
    using System.Collections.Generic;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Polaris;
    using AppliedSystems.RiskCapture.Messages;

    public class ProductLineState : AggregateState
    {
        public ProductLineCode ProductLine { get; }
        public Dictionary<string, Section> Sections { get; }

        public ProductLineState(AggregateId id, ProductLineCode productLine) : base(id)
        {
            ProductLine = productLine;
            Sections = new Dictionary<string, Section>();
        }

        public void Apply(NewRiskSectionMapped @event)
        {
            if (@event.ProductLine != ProductLine)
            {
                return;
            }
            Sections[@event.SectionName] = new Section(Id, @event.RiskSectionId, @event.ProductLine);
        }
    }
}