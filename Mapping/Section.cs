namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.RatingHub.Xml.Attributes;
    using AppliedSystems.RiskCapture.Messages;

    public class Section : AggregateEntity<SectionState>
    {
        public Section(AggregateId id, Guid riskSectionId, string productLine) 
            : base(new SectionState(id, riskSectionId, productLine))
        {
        }

        public void ExtractMapFromRiskSection(XElement sectionElement)
        {
            GreenLogger.Log("Extracting map from risk section for {0}", sectionElement.Name.LocalName);

            foreach (XElement riskItemElement in sectionElement.Elements())
            {
                if (!State.Items.ContainsKey(riskItemElement.Name.LocalName) && riskItemElement.Attributes().Any(e => e.Name.LocalName == ValAttribute.AttributeName))
                {
                    GreenLogger.Log("new risk item mapped for {0}", State.RiskSectionId, riskItemElement.Name.LocalName);

                    Then(new NewRiskItemMapped(Guid.NewGuid(), State.ProductLine, State.RiskSectionId, sectionElement.Name.LocalName, riskItemElement.Name.LocalName));
                }
            }
        }
        public void ExtractCaptureFromRiskSection(XElement sectionElement, Action<string, Guid, Guid, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk section for {0}", sectionElement.Name.LocalName);

            foreach (XElement riskItemElement in sectionElement.Elements())
            {
                if (State.Items.ContainsKey(riskItemElement.Name.LocalName) )
                {
                    State.Items[riskItemElement.Name.LocalName].ExtractCaptureFromRiskItem(riskItemElement, onValueExtraction);
                }
            }
        }
    }
}