namespace AppliedSystems.RiskCapture.Service
{
    public class RiskCaptureId
    {
        private readonly string id;

        public static RiskCaptureId Parse(int id)
        {
            return new RiskCaptureId("RiskCapture|" + id);
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