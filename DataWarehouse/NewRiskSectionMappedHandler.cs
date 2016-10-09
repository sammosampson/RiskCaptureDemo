namespace AppliedSystems.DataWarehouse
{
    using Messaging.Infrastructure.Events;
    using RiskCapture.Messages;

    public class NewRiskSectionMappedHandler : IEventHandler<NewRiskSectionMapped>
    {
        public void Handle(NewRiskSectionMapped message)
        {
        }
    }
}