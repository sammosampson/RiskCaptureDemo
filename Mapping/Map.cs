namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    using AppliedSystems.Polaris;
    using AppliedSystems.RatingHub.Xml.Body.Requests;
    using AppliedSystems.RiskCapture.Messages;
    using AppliedSystems.Xml;

    public class Map : AggregateRoot<MapState>
    {
        public Map() : base(new MapState())
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