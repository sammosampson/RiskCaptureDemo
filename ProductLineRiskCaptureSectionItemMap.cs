namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.RatingHub.Xml.Attributes;
    using Infrastucture.Messaging.EventSourcing;

    public class ProductLineRiskCaptureSectionItemMap : AggregateEntity<RiskCaptureMap>
    {
        private readonly string productLine;
        private readonly int riskSectionId;
        private readonly int riskItemId;

        public ProductLineRiskCaptureSectionItemMap(RiskCaptureMap root, string productLine, int riskSectionId, int riskItemId)
            : base(root)
        {
            this.productLine = productLine;
            this.riskSectionId = riskSectionId;
            this.riskItemId = riskItemId;
        }

        public void ExtractCaptureFromRiskItem(XElement riskItem, Action<string, int, int, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk item for {0}", riskItem.Name.LocalName);

            onValueExtraction(
                productLine, 
                riskSectionId, 
                riskItemId,
                riskItem.Attributes().Single(e => e.Name.LocalName == ValAttribute.AttributeName).Value);
        }
    }
}