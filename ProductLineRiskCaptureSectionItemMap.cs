namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.RatingHub.Xml.Attributes;
    using Infrastucture.Messaging.EventSourcing;

    public class ProductLineRiskCaptureSectionItemMap : AggregateEntity<ProductLineRiskCaptureSectionItemMapState>
    {

        public ProductLineRiskCaptureSectionItemMap(AggregateId id, string productLine, Guid riskSectionId, Guid riskItemId)
            : base(new ProductLineRiskCaptureSectionItemMapState(id, productLine, riskSectionId, riskItemId))
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