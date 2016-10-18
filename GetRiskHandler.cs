namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Messaging.Infrastructure.Requests;

    public class GetRiskHandler : IRequestHandler<GetRisk, GetRiskResponse>
    {
        public GetRiskResponse Handle(GetRisk message)
        {
            return new GetRiskResponse("Blah");
        }
    }
}