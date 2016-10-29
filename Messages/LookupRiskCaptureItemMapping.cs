namespace AppliedSystems.RiskCapture.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class LookupRiskCaptureItemMapping : IRequest
    {
        public string ProductLine { get; }
        public Guid ItemId { get; }

        public LookupRiskCaptureItemMapping(string productLine, Guid itemId)
        {
            ProductLine = productLine;
            ItemId = itemId;
        }
    }
}