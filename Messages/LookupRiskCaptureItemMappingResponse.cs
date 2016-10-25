namespace AppliedSystems.RiskCapture.Messages
{
    using System;
    using AppliedSystems.Messaging.Messages;

    public class LookupRiskCaptureItemMappingResponse : IResponse
    {
        public Guid ItemId { get; set; }
        public string SectionName { get; set; }
        public string ItemName { get; set; }
    }
}