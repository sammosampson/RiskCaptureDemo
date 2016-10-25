namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming
                    .ForRequests.Handle<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>().With<LookupRiskCaptureItemMappingHandler>()
                .Internal
                    .ForCommands.Handle<ProcessRiskCapture>().With<ProcessRiskCaptureHandler>();
        }
    }
}