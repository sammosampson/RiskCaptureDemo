namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.RatingHub.Xml.Attributes;

    public class SectionItem : AggregateEntity<SectionItemState>
    {

        public SectionItem(AggregateId id, string productLine, Guid riskSectionId, Guid riskItemId)
            : base(new SectionItemState(id, productLine, riskSectionId, riskItemId))
        {
        }

        public void ExtractCaptureFromRiskItem(XElement riskItem, Action<string, Guid, Guid, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk item for {0}", riskItem.Name.LocalName);

            onValueExtraction(
                State.ProductLine,
                State.RiskSectionId,
                State.RiskItemId,
                riskItem.Attributes().Single(e => e.Name.LocalName == ValAttribute.AttributeName).Value);
        }
    }
}