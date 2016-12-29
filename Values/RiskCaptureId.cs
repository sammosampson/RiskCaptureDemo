namespace AppliedSystems.RiskCapture.Values
{
    using System;
    using AppliedSystems.Domain.EventSourced;

    public class RiskCaptureId : AggregateId
    {
        public static RiskCaptureId Parse(string sequenceId)
        {
            return new RiskCaptureId(sequenceId);
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

        public override string ConvertToStreamName()
        {
            return "riskcapture-" + id;
        }
    }
}