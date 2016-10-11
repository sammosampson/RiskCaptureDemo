namespace AppliedSystems.RiskCapture
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class RiskItemValueCaptured : IEvent
    {
        public Guid RiskCaptureId { get; set; }
        public string ProductLine { get; set; }
        public int RiskSectionId { get; set; }
        public int ItemId { get; set; }
        public string Value { get; set; }

        public RiskItemValueCaptured(Guid riskCaptureId, string productLine, int riskSectionId, int itemId, string value)
        {
            RiskCaptureId = riskCaptureId;
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            ItemId = itemId;
            Value = value;
        }
    }
}