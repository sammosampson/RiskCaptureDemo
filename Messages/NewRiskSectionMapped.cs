namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class NewRiskSectionMapped : IEvent
    {
        public int RiskSectionId { get; }
        public string SectionName { get; set; }
        public string ProductLine { get; }
        public NewRiskSectionMapped(string productLine, int riskSectionId, string sectionName)
        {
            ProductLine = productLine;
            RiskSectionId = riskSectionId;
            SectionName = sectionName;
        }
    }
}