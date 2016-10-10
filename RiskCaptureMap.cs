namespace AppliedSystems.RiskCapture
{
    using System.Collections.Generic;
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Polaris;
    using RatingHub.Xml.Body.Requests;
    using Xml;

    public class RiskCaptureMap : AggregateRoot
    {
        private readonly Dictionary<ProductLineCode, ProductLineRiskCaptureMap> productLines;

        public RiskCaptureMap()
        {
            productLines = new Dictionary<ProductLineCode, ProductLineRiskCaptureMap>();
        }

        public void ExtractMapFromRequest(string request)
        {
            RequestBodyElement body = request.ToXDocument().GetRequestBody();
            ProductLineCode code = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);

            if (!productLines.ContainsKey(code))
            {
                Then(new NewRiskProductLineMapped(code));
            }
            productLines[code].ExtractMapFromRisk(body.PolMessage.InputPolData);
        }

        public void Apply(NewRiskProductLineMapped @event)
        {
            var code = ProductLineCode.Parse(@event.ProductLine);
            productLines[code] = new ProductLineRiskCaptureMap(this, code);
        }
    }
}