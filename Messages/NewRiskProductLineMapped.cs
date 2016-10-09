namespace AppliedSystems.RiskCapture.Messages
{
    using Messaging.Messages;

    public class NewRiskProductLineMapped : IEvent
    {
        public string ProductLine { get; }

        public NewRiskProductLineMapped(string productLine)
        {
            ProductLine = productLine;
        }
    }
}