namespace AppliedSystems.RiskCapture
{
    using System;

    public class RiskCaptureId
    {
        public static RiskCaptureId Parse(string sequenceId)
        {
            return new RiskCaptureId(sequenceId);
        }

        public static implicit operator string(RiskCaptureId from)
        {
            return "riskcapture-" + from.id;
        }

        public static implicit operator Guid(RiskCaptureId from)
        {
            return new Guid(from.id);
        }

        private readonly string id;

        private RiskCaptureId(string id)
        {
            this.id = id;
        }
    }
}