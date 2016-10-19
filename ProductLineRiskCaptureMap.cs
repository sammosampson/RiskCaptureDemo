namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using AppliedSystems.Infrastucture;
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Polaris;
    using RatingHub.Xml.Body.PolMessage.PolData;

    public class ProductLineRiskCaptureMap : AggregateEntity<RiskCaptureMap>
    {
        private readonly ProductLineCode productLine;
        private readonly Dictionary<string, ProductLineRiskCaptureSectionMap> sections;
        private int lastSectionId;

        public ProductLineRiskCaptureMap(RiskCaptureMap root, ProductLineCode productLine) : base(root)
        {
            this.productLine = productLine;
            sections = new Dictionary<string, ProductLineRiskCaptureSectionMap>();
        }

        public void ExtractMapFromRisk(InputPolDataElement risk)
        {
            GreenLogger.Log("Extracting map from risk for {0}", productLine);
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                if (!sections.ContainsKey(riskSectionElement.Name.LocalName))
                {
                    GreenLogger.Log("new risk section mapped for {0}", riskSectionElement.Name.LocalName);
                    Then(new NewRiskSectionMapped(productLine, ++lastSectionId, riskSectionElement.Name.LocalName));
                }

                sections[riskSectionElement.Name.LocalName].ExtractMapFromRiskSection(riskSectionElement);
            }
        }

        public void ExtractCaptureFromRisk(InputPolDataElement risk, Action<string, int, int, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk for {0}", productLine);
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                sections[riskSectionElement.Name.LocalName].ExtractCaptureFromRiskSection(riskSectionElement, onValueExtraction);
            }
        }

        public void Apply(NewRiskSectionMapped @event)
        {
            if (@event.ProductLine != productLine)
            {
                return;
            }
            sections[@event.SectionName] = new ProductLineRiskCaptureSectionMap(Root, @event.RiskSectionId, @event.ProductLine);
            lastSectionId = @event.RiskSectionId;
        }
    }
}