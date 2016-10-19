namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.RatingHub.Xml.Attributes;
    using AppliedSystems.RiskCapture.Messages;

    public class ProductLineRiskCaptureSectionMap : AggregateEntity<RiskCaptureMap>
    {
        private readonly Dictionary<string, ProductLineRiskCaptureSectionItemMap> items;
        private readonly int riskSectionId;
        private readonly string productLine;
        private int lastItemId;

        public ProductLineRiskCaptureSectionMap(RiskCaptureMap root, int riskSectionId, string productLine) : base(root)
        {
            this.riskSectionId = riskSectionId;
            this.productLine = productLine;
            items = new Dictionary<string, ProductLineRiskCaptureSectionItemMap>();
        }

        public void ExtractMapFromRiskSection(XElement sectionElement)
        {
            GreenLogger.Log("Extracting map from risk section for {0}", sectionElement.Name.LocalName);

            foreach (XElement riskItemElement in sectionElement.Elements())
            {
                if (!items.ContainsKey(riskItemElement.Name.LocalName) && riskItemElement.Attributes().Any(e => e.Name.LocalName == ValAttribute.AttributeName))
                {
                    GreenLogger.Log("new risk item mapped for {0}", riskSectionId, riskItemElement.Name.LocalName);
                    Then(new NewRiskItemMapped(++lastItemId, productLine, riskSectionId, riskItemElement.Name.LocalName));
                }
            }
        }
        public void ExtractCaptureFromRiskSection(XElement sectionElement, Action<string, int, int, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk section for {0}", sectionElement.Name.LocalName);

            foreach (XElement riskItemElement in sectionElement.Elements())
            {
                if (items.ContainsKey(riskItemElement.Name.LocalName) )
                {
                    items[riskItemElement.Name.LocalName].ExtractCaptureFromRiskItem(riskItemElement, onValueExtraction);
                }
            }
        }

        public void Apply(NewRiskItemMapped @event)
        {
            if (@event.ProductLine != productLine || @event.RiskSectionId != riskSectionId)
            {
                return;
            }

            items[@event.ItemName] = new ProductLineRiskCaptureSectionItemMap(Root, @event.ProductLine, @event.RiskSectionId, @event.RiskItemId);
            lastItemId = @event.RiskItemId;
        }

    }
}