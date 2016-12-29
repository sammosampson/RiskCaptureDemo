namespace AppliedSystems.RiskCapture.Mapping
{
    using AppliedSystems.Domain.EventSourced;

    public class MapId : AggregateId
    {
        public override string ConvertToStreamName()
        {
            return "riskcapture-riskcapturemap";
        }
    }
}