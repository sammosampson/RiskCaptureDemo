namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class NewRiskItemMapped : IEvent
    {
        public int RiskItemId { get; }
        public string ProductLine { get; }
        public int RiskSectionId { get; }
        public string ItemName { get; }

        public NewRiskItemMapped(int riskItemId, string productLine, int riskSectionId, string itemName)
        {
            RiskItemId = riskItemId;
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            ItemName = itemName;
        }
    }
}