namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Messaging.Messages;

    public class GetRiskResponse : IResponse
    {
        public string Risk { get; set; }

        public GetRiskResponse(string risk)
        {
            Risk = risk;
        }
    }
}