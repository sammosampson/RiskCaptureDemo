namespace AppliedSystems.RiskCapture
{
    using System;
    using System.Collections.Generic;
    using AppliedSystems.Infrastucture;
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
            GreenLogger.Log("Extract risk capture map from request");

            RequestBodyElement body = request.ToXDocument().GetRequestBody();
            ProductLineCode productLine = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);

            if (!productLines.ContainsKey(productLine))
            {
                GreenLogger.Log("new risk product line mapped for {0}", productLine);
                Then(new NewRiskProductLineMapped(productLine));
            }
            productLines[productLine].ExtractMapFromRisk(body.PolMessage.InputPolData);
        }

        public void Apply(NewRiskProductLineMapped @event)
        {
            var productLine = ProductLineCode.Parse(@event.ProductLine);
            productLines[productLine] = new ProductLineRiskCaptureMap(this, productLine);
        }

        public void ExtractCaptureFromRequest(string request, Action<string, int, int, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from request");

            RequestBodyElement body = request.ToXDocument().GetRequestBody();
            ProductLineCode code = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);
            productLines[code].ExtractCaptureFromRisk(body.PolMessage.InputPolData, onValueExtraction);

        }
    }
}