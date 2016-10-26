namespace AppliedSystems.RiskCapture.Mapping
{
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;

    public class MapId : AggregateId
    {
        public override string ConvertToStreamName()
        {
            return "riskcapture-riskcapturemap";
        }
    }
}