namespace AppliedSystems.RiskCapture
{
    public class RiskCaptureId
    {
        private readonly string id;

        public static RiskCaptureId Parse(int id)
        {
            return new RiskCaptureId("riskcapture-" + id);
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