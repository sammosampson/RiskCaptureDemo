namespace AppliedSystems.RiskCapture.Messages
{
    using System;
    using Messaging.Messages;

    public class NewRiskSectionMapped : IEvent
    {
        public Guid RiskSectionId { get; }
        public string SectionName { get; set; }
        public string ProductLine { get; }
        public NewRiskSectionMapped(string productLine, Guid riskSectionId, string sectionName)
        {
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            SectionName = sectionName;
        }
    }
}