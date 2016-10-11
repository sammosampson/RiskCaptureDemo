namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Polaris;
    using RatingHub.Xml.Body.PolMessage.PolData;

    public class ProductLineRiskCaptureMap : AggregateEntity<RiskCaptureMap>
    {
        private readonly ProductLineCode code;
        private readonly Dictionary<string, ProductLineRiskCaptureSectionMap> sections;
        private int lastId;

        public ProductLineRiskCaptureMap(RiskCaptureMap root, ProductLineCode code) : base(root)
        {
            this.code = code;
            sections = new Dictionary<string, ProductLineRiskCaptureSectionMap>();
        }

        public void ExtractMapFromRisk(InputPolDataElement risk)
        {
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                if (!sections.ContainsKey(riskSectionElement.Name.LocalName))
                {
                    Then(new NewRiskSectionMapped(code, ++lastId, riskSectionElement.Name.LocalName));
                }

                sections[riskSectionElement.Name.LocalName].ExtractMapFromRiskSection(riskSectionElement);
            }
        }

        public void ExtractCaptureFromRisk(InputPolDataElement risk, Action<string, int, int, string> onValueExtraction)
        {
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                sections[riskSectionElement.Name.LocalName].ExtractCaptureFromRiskSection(riskSectionElement, onValueExtraction);
            }
        }

        public void Apply(NewRiskSectionMapped @event)
        {
            sections[@event.SectionName] = new ProductLineRiskCaptureSectionMap(Root, @event.RiskSectionId, @event.ProductLine);
            lastId = @event.RiskSectionId;
        }
    }
}