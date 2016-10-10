namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class NewRiskItemMapped : IEvent
    {
        public int RiskItemId { get; }
        public int RiskSectionId { get; }
        public string ItemName { get; }

        public NewRiskItemMapped(int riskItemId, int riskSectionId, string itemName)
        {
            RiskItemId = riskItemId;
            RiskSectionId = riskSectionId;
            ItemName = itemName;
        }
    }
}