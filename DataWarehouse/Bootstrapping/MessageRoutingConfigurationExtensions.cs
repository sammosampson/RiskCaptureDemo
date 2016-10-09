namespace AppliedSystems.DataWarehouse.Bootstrapping
{
    using Messaging.Infrastructure.Bootstrapping;
    using RiskCapture.Messages;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming.ForEvents
                    .Handle<RiskCaptureProcessed>().With<RiskCaptureProcessedHandler>()
                    .Handle<NewRiskProductLineMapped>().With<NewRiskProductLineMappedHandler>()
                    .Handle<NewRiskSectionMapped>().With<NewRiskSectionMappedHandler>();
        }
    }
}