namespace AppliedSystems.RiskCapture.Bootstrapping
{
    using AppliedSystems.RiskCapture.Mapping;
    using Messages;
    using Messaging.Infrastructure.Bootstrapping;

    public static class MessageRoutingConfigurationExtensions
    {
        public static MessageRoutingConfiguration WireUpRouting(this MessageRoutingConfiguration config)
        {
            return config
                .Incoming
                    .ForRequests.Handle<LookupRiskCaptureItemMapping, LookupRiskCaptureItemMappingResponse>().With<LookupItemMappingHandler>()
                .Internal
                    .ForCommands.Handle<ProcessRiskCapture>().With<ProcessRiskCaptureHandler>();
        }
    }
}