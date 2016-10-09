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

        public void ExtractMapFromRiskSection(InputPolDataElement risk)
        {
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                if (!sections.ContainsKey(riskSectionElement.Value))
                {
                    Then(new NewRiskSectionMapped(code, lastId++, riskSectionElement.Value));
                }
            }
        }

        public void Apply(NewRiskSectionMapped @event)
        {
            sections[@event.SectionName] = new ProductLineRiskCaptureSectionMap(Root);
        }
    }

    public class ProductLineRiskCaptureSectionMap : AggregateEntity<RiskCaptureMap>
    {
        public ProductLineRiskCaptureSectionMap(RiskCaptureMap root) : base(root)
        {
        }
    }
}