namespace AppliedSystems.DataWarehouse
{
    using System;
    using Messaging.Infrastructure.Events;
    using RiskCapture.Messages;

    public class RiskCaptureProcessedHandler : IEventHandler<RiskCaptureProcessed>
    {
        public void Handle(RiskCaptureProcessed message)
        {
        }
    }
}