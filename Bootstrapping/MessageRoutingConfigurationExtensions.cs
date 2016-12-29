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
                .Internal
                    .ForCommands.Handle<ProcessRiskCapture>().With<ProcessRiskCaptureHandler>();
        }
    }
}