namespace AppliedSystems.RiskCapture
{
    using AppliedSystems.Infrastucture.Messaging.EventSourcing;
    public class RiskCaptureMapId : AggregateId
    {
        public override string ConvertToStreamName()
        {
            return "riskcapture-riskcapturemap";
        }
    }
}