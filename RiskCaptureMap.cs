namespace AppliedSystems.RiskCapture
{
    using System;
    using AppliedSystems.Infrastucture;
    using Infrastucture.Messaging.EventSourcing;
    using Messages;
    using Polaris;
    using RatingHub.Xml.Body.Requests;
    using Xml;

    public class RiskCaptureMap : AggregateRoot<RiskCaptureMapState>
    {
        public RiskCaptureMap() : base(new RiskCaptureMapState())
        {
        }

        public void ExtractMapFromRequest(string request)
        {
            GreenLogger.Log("Extract risk capture map from request");

            RequestBodyElement body = request.ToXDocument().GetRequestBody();
            ProductLineCode productLine = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);

            if (!State.ProductLines.ContainsKey(productLine))
            {
                GreenLogger.Log("new risk product line mapped for {0}", productLine);
                Then(new NewRiskProductLineMapped(productLine));
            }

            State.ProductLines[productLine].ExtractMapFromRisk(body.PolMessage.InputPolData);
        }

        public void ExtractCaptureFromRequest(string request, Action<string, Guid, Guid, string> onValueExtraction)
        {
            GreenLogger.Log("Extracting capture from request");

            RequestBodyElement body = request.ToXDocument().GetRequestBody();
            ProductLineCode code = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);

            State.ProductLines[code].ExtractCaptureFromRisk(body.PolMessage.InputPolData, onValueExtraction);

        }
    }
}