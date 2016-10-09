namespace AppliedSystems.DataWarehouse
{
    using Messaging.Infrastructure.Events;
    using RiskCapture.Messages;

    public class NewRiskProductLineMappedHandler : IEventHandler<NewRiskProductLineMapped>
    {
        public void Handle(NewRiskProductLineMapped message)
        {
        }
    }
}