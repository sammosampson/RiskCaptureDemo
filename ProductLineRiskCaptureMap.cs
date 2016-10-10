namespace AppliedSystems.RiskCapture
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Polaris;
    using RatingHub.Xml.Attributes;
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

        public void Apply(NewRiskSectionMapped @event)
        {
            sections[@event.SectionName] = new ProductLineRiskCaptureSectionMap(Root, @event.RiskSectionId);
            lastId = @event.RiskSectionId;
        }
    }

    public class ProductLineRiskCaptureSectionMap : AggregateEntity<RiskCaptureMap>
    {
        private readonly Dictionary<string, ProductLineRiskCaptureSectionItemMap> items;
        private readonly int riskSectionId;
        private int lastId;

        public ProductLineRiskCaptureSectionMap(RiskCaptureMap root, int riskSectionId) : base(root)
        {
            this.riskSectionId = riskSectionId;
            items = new Dictionary<string, ProductLineRiskCaptureSectionItemMap>();
        }

        public void ExtractMapFromRiskSection(XElement sectionElement)
        {
            foreach (XElement riskItemElement in sectionElement.Elements())
            {
                if (!items.ContainsKey(riskItemElement.Name.LocalName) && riskItemElement.Attributes().Any(e => e.Name.LocalName == ValAttribute.AttributeName))
                {
                    Then(new NewRiskItemMapped(++lastId, riskSectionId, riskItemElement.Name.LocalName));
                }
            }
        }

        public void Apply(NewRiskItemMapped @event)
        {
            items[@event.ItemName] = new ProductLineRiskCaptureSectionItemMap(Root);
            lastId = @event.RiskItemId;
        }
    }
}