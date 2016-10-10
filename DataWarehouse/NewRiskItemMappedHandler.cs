namespace AppliedSystems.DataWarehouse
{
    using Messaging.Infrastructure.Events;
    using RiskCapture.Messages;

    public class NewRiskItemMappedHandler : IEventHandler<NewRiskItemMapped>
    {
        public void Handle(NewRiskItemMapped message)
        {
        }
    }
}