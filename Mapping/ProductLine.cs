namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using System.Xml.Linq;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Polaris;
    using AppliedSystems.RatingHub.Xml.Body.PolMessage.PolData;
    using AppliedSystems.RiskCapture.Messages;

    public class ProductLine : AggregateEntity<ProductLineState>
    {
        public ProductLine(AggregateId id, ProductLineCode productLine) 
            : base(new ProductLineState(id, productLine))
        {
        }

        public void ExtractMapFromRisk(InputPolDataElement risk)
        {
            GreenLogger.Log("Extracting map from risk for {0}", State.ProductLine);
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                if (!State.Sections.ContainsKey(riskSectionElement.Name.LocalName))
                {
                    GreenLogger.Log("new risk section mapped for {0}", riskSectionElement.Name.LocalName);

                    Then(new NewRiskSectionMapped(State.ProductLine, Guid.NewGuid(), riskSectionElement.Name.LocalName));
                }

                State.Sections[riskSectionElement.Name.LocalName].ExtractMapFromRiskSection(riskSectionElement);
            }
        }

        public void ExtractCaptureFromRisk(InputPolDataElement risk, Action<string, Guid, Guid, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from risk for {0}", State.ProductLine);
            foreach (XElement riskSectionElement in risk.InnerXElement.Elements())
            {
                State.Sections[riskSectionElement.Name.LocalName].ExtractCaptureFromRiskSection(riskSectionElement, onValueExtraction);
            }
        }
    }
}