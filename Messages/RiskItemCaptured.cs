namespace AppliedSystems.RiskCapture.Messages
{
    using System;

    public class RiskItemCaptured
    {
        public Guid CustomerId { get; }
        public int RiskItemId { get; }
        public string Value { get; }
    }
}