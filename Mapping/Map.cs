namespace AppliedSystems.RiskCapture.Mapping
{
    using System;
    using AppliedSystems.Domain.EventSourced;
    using AppliedSystems.Infrastucture;
    using AppliedSystems.Polaris;
    using AppliedSystems.RatingHub.Xml;
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

            RequestBodyElement body = request.ToXDocument().GetInsureServeMessage().RequestBody;
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

            RequestBodyElement body = request.ToXDocument().GetInsureServeMessage().RequestBody;
            ProductLineCode code = ProductLineCode.Parse(body.BusinessTransaction.ProductLineCode.Value);

            State.ProductLines[code].ExtractCaptureFromRisk(body.PolMessage.InputPolData, onValueExtraction);

        }
    }
}