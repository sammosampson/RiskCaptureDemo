namespace AppliedSystems.RiskCapture.Service
{
    using AppliedSystems.RiskCapture.Infrastucture.Messaging.EventSourcing;

    public class RiskCaptureId
    {
        private readonly string id;

        public static RiskCaptureId Parse(int id)
        {
            return new RiskCaptureId("Test");
        }

        private RiskCaptureId(string id)
        {
            this.id = id;
        }

        public static implicit operator string(RiskCaptureId from)
        {
            return from.id;
        }
    }
}