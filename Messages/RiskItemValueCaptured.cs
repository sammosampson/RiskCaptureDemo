namespace AppliedSystems.RiskCapture.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class RiskItemValueCaptured : IEvent
    {
        public Guid RiskCaptureId { get; }
        public string ProductLine { get; }
        public Guid RiskSectionId { get; }
        public Guid ItemId { get;  }
        public string Value { get; }

        public RiskItemValueCaptured(Guid riskCaptureId, string productLine, Guid riskSectionId, Guid itemId, string value)
        {
            RiskCaptureId = riskCaptureId;
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            ItemId = itemId;
            Value = value;
        }
    }
}