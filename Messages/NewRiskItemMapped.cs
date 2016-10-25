namespace AppliedSystems.RiskCapture.Messages
{
    using System;
    using Messaging.Messages;

    public class NewRiskItemMapped : IEvent
    {
        public Guid RiskItemId { get; }
        public string ProductLine { get; }
        public Guid RiskSectionId { get; }
        public string SectionName { get; }
        public string ItemName { get; }

        public NewRiskItemMapped(Guid riskItemId, string productLine, Guid riskSectionId, string sectionName, string itemName)
        {
            RiskItemId = riskItemId;
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            SectionName = sectionName;
            ItemName = itemName;
        }
    }
}